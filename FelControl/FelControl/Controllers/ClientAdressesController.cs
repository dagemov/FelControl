using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FelControl;

namespace FelControl.Controller
{
	public class ClientAdressesController : Controller
	{
		private IUnitOfWorkFactory uowFactory;
		private IRepository<ClientAdress> repository;
		private IRepository<Client> ClientRepository;

		public ClientAdressesController()
		{
			ApplicationDbContext context = new ApplicationDbContext();
			this.uowFactory = new EntityFrameworkUnitOfWorkFactory(context);
			this.repository = new EntityFrameworkRepository<ClientAdress>(context);
			this.ClientRepository = new EntityFrameworkRepository<Client>(context);
		}
		
    public ClientAdressesController(IUnitOfWorkFactory uowFactory, IRepository<ClientAdress> repository , IRepository<Client> ClientRepository)
		{
			this.uowFactory = uowFactory;
			this.repository = repository;
			this.ClientRepository = ClientRepository;
		}

		//
		// GET: /ClientAdresses

		public ViewResult Index(int? page, int? pageSize, string sortBy, bool? sortDesc , int? ClientId)
		{
			// Defaults
			if (!page.HasValue)
				page = 1;
			if (!pageSize.HasValue)
				pageSize = 10;

			IQueryable<ClientAdress> query = repository.All();
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
				if (sortBy == "Id")
					query = OrderBy(query, x => x.Id, ascending);
				ViewBag.SortBy = sortBy;
				if (sortDesc != null && sortDesc.Value)
					ViewBag.SortDesc = sortDesc.Value;
			}

			ViewBag.Entities = query.ToList();
			return View();
		}

		//
		// GET: /ClientAdresses/Create

		public ActionResult Create()
		{
			List<SelectListItem> selectList;
			selectList = new List<SelectListItem>();
			selectList.AddRange(ClientRepository.All().ToList().Select(x => new SelectListItem() { Text = x.LastName.ToString(), Value = x.Id.ToString(), Selected = null == x.Id }));
			ViewBag.Client = selectList;
		    return View();
		} 
		
		//
		// POST: /ClientAdresses/Create
		
		[HttpPost]
		public ActionResult Create(ClientAdress entity)
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
		// GET: /ClientAdresses/Details
		
		public ViewResult Details(int Id, int ClientId)
		{
			return View(repository.All().Single(x => x.Id == Id && x.ClientId == ClientId));
		}


		//
		// GET: /ClientAdresses/Edit
				
		public ActionResult Edit(int Id, int ClientId)
		{
			var entity = repository.All().Single(x => x.Id == Id && x.ClientId == ClientId);
			List<SelectListItem> selectList;
			selectList = new List<SelectListItem>();
			selectList.AddRange(ClientRepository.All().ToList().Select(x => new SelectListItem() { Text = x.LastName.ToString(), Value = x.Id.ToString(), Selected = entity.ClientId == x.Id }));
			ViewBag.Client = selectList;
			return View(entity);
		}
				
		//
		// POST: /ClientAdresses/Edit
				
		[HttpPost]
		public ActionResult Edit(ClientAdress entity)
		{
			if (ModelState.IsValid)
				using (IUnitOfWork uow = uowFactory.Create()) {
					ClientAdress original = repository.All().Single(x => x.Id == entity.Id && x.ClientId == entity.ClientId);
					original.Id = entity.Id;
					original.ClientId = entity.ClientId;
					original.Adress = entity.Adress;
					uow.Save();
					return RedirectToAction("Index");
				}
			else
				return View();
		}
		
		//
		// GET: /ClientAdresses/Delete
		
		public ActionResult Delete(int Id, int ClientId)
		{
			return View(repository.All().Single(x => x.Id == Id && x.ClientId == ClientId));
		}
		
		//
		// POST: /ClientAdresses/Delete
		
		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int Id, int ClientId)
		{
			using (IUnitOfWork uow = uowFactory.Create()) {
				repository.Remove(repository.All().Single(x => x.Id == Id && x.ClientId == ClientId));
				uow.Save();
				return RedirectToAction("Index");
			}
		}

		private static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(IQueryable<TSource> source, System.Linq.Expressions.Expression<Func<TSource, TKey>> keySelector, bool ascending) {

			return ascending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
		}
	}
}

