using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FelControl;

namespace FelControl.Controller
{
	public class ServicesController : Controller
	{
		private IUnitOfWorkFactory uowFactory;
		private IRepository<Service> repository;
		private IRepository<Client> ClientRepository;

		public ServicesController()
		{
			ApplicationDbContext context = new ApplicationDbContext();
			this.uowFactory = new EntityFrameworkUnitOfWorkFactory(context);
			this.repository = new EntityFrameworkRepository<Service>(context);
			this.ClientRepository = new EntityFrameworkRepository<Client>(context);
		}
		
    public ServicesController(IUnitOfWorkFactory uowFactory, IRepository<Service> repository , IRepository<Client> ClientRepository)
		{
			this.uowFactory = uowFactory;
			this.repository = repository;
			this.ClientRepository = ClientRepository;
		}

		//
		// GET: /Services

		public ViewResult Index(int? page, int? pageSize, string sortBy, bool? sortDesc , int? ClientId)
		{
			// Defaults
			if (!page.HasValue)
				page = 1;
			if (!pageSize.HasValue)
				pageSize = 10;

			IQueryable<Service> query = repository.All();
			query = query.OrderBy(x => x.Id);
			// Filtering
			List<SelectListItem> selectList;
			if (ClientId != null) {
				query = query.Where(x => x.ClientId == ClientId);
				ViewBag.ClientId = ClientId;
			}
			selectList = new List<SelectListItem>();
			selectList.Add(new SelectListItem() { Text = null, Value = null, Selected = ClientId == null });
			selectList.AddRange(ClientRepository.All().ToList().Select(x => new SelectListItem() { Text = x.LastName.ToString(), Value = x.Id.ToString(), Selected = ClientId != null && ClientId == x.Id }));
			ViewBag.Clients = selectList;
			ViewBag.SelectedClient = selectList.Where(x => x.Selected).Select(x => x.Text).FirstOrDefault();
			
			// Paging
			int pageCount = (int)((query.Count() + pageSize - 1) / pageSize);
			if (page > 1)
				query = query.Skip((page.Value - 1) * pageSize.Value);
			query = query.Take(pageSize.Value);
			if (page > 1)
				ViewBag.Page = page.Value;
			if (pageSize != 10)
				ViewBag.PageSize = pageSize.Value;
			if (pageCount > 1) {
				int currentPage = page.Value;
				const int visiblePages = 5;
				const int pageDelta = 2;
				List<Tuple<string, bool, int>> paginationData = new List<Tuple<string, bool, int>>(); // text, enabled, page index
				paginationData.Add(new Tuple<string, bool, int>("Prev", currentPage > 1, currentPage - 1));
				if (pageCount <= visiblePages * 2) {
					for (int i = 1; i <= pageCount; i++)
						paginationData.Add(new Tuple<string, bool, int>(i.ToString(), true, i));
				}
				else {
					if (currentPage < visiblePages) {
						// 12345..10
						for (int i = 1; i <= visiblePages; i++)
							paginationData.Add(new Tuple<string, bool, int>(i.ToString(), true, i));
						paginationData.Add(new Tuple<string, bool, int>("...", false, -1));
						paginationData.Add(new Tuple<string, bool, int>(pageCount.ToString(), true, pageCount));
					}
					else if (currentPage > pageCount - (visiblePages - 1)) {
						// 1..678910
						paginationData.Add(new Tuple<string, bool, int>("1", true, 1));
						paginationData.Add(new Tuple<string, bool, int>("...", false, -1));
						for (int i = pageCount - (visiblePages - 1); i <= pageCount; i++)
							paginationData.Add(new Tuple<string, bool, int>(i.ToString(), true, i));
					}
					else {
						// 1..34567..10
						paginationData.Add(new Tuple<string, bool, int>("1", true, 1));
						paginationData.Add(new Tuple<string, bool, int>("...", false, -1));
						for (int i = currentPage - pageDelta, count = currentPage + pageDelta; i <= count; i++)
							paginationData.Add(new Tuple<string, bool, int>(i.ToString(), true, i));
						paginationData.Add(new Tuple<string, bool, int>("...", false, -1));
						paginationData.Add(new Tuple<string, bool, int>(pageCount.ToString(), true, pageCount));
					}
				}
				paginationData.Add(new Tuple<string, bool, int>("Next", currentPage < pageCount, currentPage + 1));
				ViewBag.PaginationData = paginationData;
			}

			// Sorting
			if (!string.IsNullOrEmpty(sortBy)) {
				bool ascending = !sortDesc.HasValue || !sortDesc.Value;
				if (sortBy == "Name")
					query = OrderBy(query, x => x.Name, ascending);
				if (sortBy == "Description")
					query = OrderBy(query, x => x.Description, ascending);
				if (sortBy == "ImagenPrincipal")
					query = OrderBy(query, x => x.ImagenPrincipal, ascending);
				if (sortBy == "Price")
					query = OrderBy(query, x => x.Price, ascending);
				ViewBag.SortBy = sortBy;
				if (sortDesc != null && sortDesc.Value)
					ViewBag.SortDesc = sortDesc.Value;
			}

			ViewBag.Entities = query.ToList();
			return View();
		}

		//
		// GET: /Services/Create

		public ActionResult Create()
		{
			List<SelectListItem> selectList;
			selectList = new List<SelectListItem>();
			selectList.AddRange(ClientRepository.All().ToList().Select(x => new SelectListItem() { Text = x.LastName.ToString(), Value = x.Id.ToString(), Selected = null == x.Id }));
			ViewBag.Client = selectList;
		    return View();
		} 
		
		//
		// POST: /Services/Create
		
		[HttpPost]
		public ActionResult Create(Service entity)
		{
			if (ModelState.IsValid)
				using (IUnitOfWork uow = uowFactory.Create()) {
					repository.Add(entity);
					uow.Save();
					return RedirectToAction("Index");
				}
			else
				return View();
		}

		//
		// GET: /Services/Details
		
		public ViewResult Details(int Id)
		{
			return View(repository.All().Single(x => x.Id == Id));
		}


		//
		// GET: /Services/Edit
				
		public ActionResult Edit(int Id)
		{
			var entity = repository.All().Single(x => x.Id == Id);
			List<SelectListItem> selectList;
			selectList = new List<SelectListItem>();
			selectList.AddRange(ClientRepository.All().ToList().Select(x => new SelectListItem() { Text = x.LastName.ToString(), Value = x.Id.ToString(), Selected = entity.ClientId == x.Id }));
			ViewBag.Client = selectList;
			return View(entity);
		}
				
		//
		// POST: /Services/Edit
				
		[HttpPost]
		public ActionResult Edit(Service entity)
		{
			if (ModelState.IsValid)
				using (IUnitOfWork uow = uowFactory.Create()) {
					Service original = repository.All().Single(x => x.Id == entity.Id);
					original.Id = entity.Id;
					original.Name = entity.Name;
					original.Description = entity.Description;
					original.ImagenPrincipal = entity.ImagenPrincipal;
					original.Price = entity.Price;
					original.ContractService = entity.ContractService;
					original.ClientId = entity.ClientId;
					uow.Save();
					return RedirectToAction("Index");
				}
			else
				return View();
		}
		
		//
		// GET: /Services/Delete
		
		public ActionResult Delete(int Id)
		{
			return View(repository.All().Single(x => x.Id == Id));
		}
		
		//
		// POST: /Services/Delete
		
		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int Id)
		{
			using (IUnitOfWork uow = uowFactory.Create()) {
				repository.Remove(repository.All().Single(x => x.Id == Id));
				uow.Save();
				return RedirectToAction("Index");
			}
		}

		private static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(IQueryable<TSource> source, System.Linq.Expressions.Expression<Func<TSource, TKey>> keySelector, bool ascending) {

			return ascending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
		}
	}
}

