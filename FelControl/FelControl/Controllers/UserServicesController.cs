using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FelControl;

namespace FelControl.Controller
{
	public class UserServicesController : Controller
	{
		private IUnitOfWorkFactory uowFactory;
		private IRepository<UserService> repository;
		private IRepository<Service> ServiceRepository;
		private IRepository<User> UserRepository;

		public UserServicesController()
		{
			ApplicationDbContext context = new ApplicationDbContext();
			this.uowFactory = new EntityFrameworkUnitOfWorkFactory(context);
			this.repository = new EntityFrameworkRepository<UserService>(context);
			this.ServiceRepository = new EntityFrameworkRepository<Service>(context);
			this.UserRepository = new EntityFrameworkRepository<User>(context);
		}
		
    public UserServicesController(IUnitOfWorkFactory uowFactory, IRepository<UserService> repository , IRepository<Service> ServiceRepository, IRepository<User> UserRepository)
		{
			this.uowFactory = uowFactory;
			this.repository = repository;
			this.ServiceRepository = ServiceRepository;
			this.UserRepository = UserRepository;
		}

		//
		// GET: /UserServices

		public ViewResult Index(int? page, int? pageSize, string sortBy, bool? sortDesc , int? ServiceId, int? UserId)
		{
			// Defaults
			if (!page.HasValue)
				page = 1;
			if (!pageSize.HasValue)
				pageSize = 10;

			IQueryable<UserService> query = repository.All();
			query = query.OrderBy(x => x.ServiceId);
			// Filtering
			List<SelectListItem> selectList;
			if (ServiceId != null) {
				query = query.Where(x => x.ServiceId == ServiceId);
				ViewBag.ServiceId = ServiceId;
			}
			selectList = new List<SelectListItem>();
			selectList.Add(new SelectListItem() { Text = null, Value = null, Selected = ServiceId == null });
			selectList.AddRange(ServiceRepository.All().ToList().Select(x => new SelectListItem() { Text = x.Name.ToString(), Value = x.Id.ToString(), Selected = ServiceId != null && ServiceId == x.Id }));
			ViewBag.Services = selectList;
			ViewBag.SelectedService = selectList.Where(x => x.Selected).Select(x => x.Text).FirstOrDefault();
			if (UserId != null) {
				query = query.Where(x => x.UserId == UserId);
				ViewBag.UserId = UserId;
			}
			selectList = new List<SelectListItem>();
			selectList.Add(new SelectListItem() { Text = null, Value = null, Selected = UserId == null });
			selectList.AddRange(UserRepository.All().ToList().Select(x => new SelectListItem() { Text = x.LastName.ToString(), Value = x.Id.ToString(), Selected = UserId != null && UserId == x.Id }));
			ViewBag.Users = selectList;
			ViewBag.SelectedUser = selectList.Where(x => x.Selected).Select(x => x.Text).FirstOrDefault();
			
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
				ViewBag.SortBy = sortBy;
				if (sortDesc != null && sortDesc.Value)
					ViewBag.SortDesc = sortDesc.Value;
			}

			ViewBag.Entities = query.ToList();
			return View();
		}

		//
		// GET: /UserServices/Create

		public ActionResult Create()
		{
			List<SelectListItem> selectList;
			selectList = new List<SelectListItem>();
			selectList.AddRange(ServiceRepository.All().ToList().Select(x => new SelectListItem() { Text = x.Name.ToString(), Value = x.Id.ToString(), Selected = null == x.Id }));
			ViewBag.Service = selectList;
			selectList = new List<SelectListItem>();
			selectList.AddRange(UserRepository.All().ToList().Select(x => new SelectListItem() { Text = x.LastName.ToString(), Value = x.Id.ToString(), Selected = null == x.Id }));
			ViewBag.User = selectList;
		    return View();
		} 
		
		//
		// POST: /UserServices/Create
		
		[HttpPost]
		public ActionResult Create(UserService entity)
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
		// GET: /UserServices/Details
		
		public ViewResult Details(int ServiceId, int UserId)
		{
			return View(repository.All().Single(x => x.ServiceId == ServiceId && x.UserId == UserId));
		}


		//
		// GET: /UserServices/Edit
				
		public ActionResult Edit(int ServiceId, int UserId)
		{
			var entity = repository.All().Single(x => x.ServiceId == ServiceId && x.UserId == UserId);
			List<SelectListItem> selectList;
			selectList = new List<SelectListItem>();
			selectList.AddRange(ServiceRepository.All().ToList().Select(x => new SelectListItem() { Text = x.Name.ToString(), Value = x.Id.ToString(), Selected = entity.ServiceId == x.Id }));
			ViewBag.Service = selectList;
			selectList = new List<SelectListItem>();
			selectList.AddRange(UserRepository.All().ToList().Select(x => new SelectListItem() { Text = x.LastName.ToString(), Value = x.Id.ToString(), Selected = entity.UserId == x.Id }));
			ViewBag.User = selectList;
			return View(entity);
		}
				
		//
		// POST: /UserServices/Edit
				
		[HttpPost]
		public ActionResult Edit(UserService entity)
		{
			if (ModelState.IsValid)
				using (IUnitOfWork uow = uowFactory.Create()) {
					UserService original = repository.All().Single(x => x.ServiceId == entity.ServiceId && x.UserId == entity.UserId);
					original.ServiceId = entity.ServiceId;
					original.UserId = entity.UserId;
					uow.Save();
					return RedirectToAction("Index");
				}
			else
				return View();
		}
		
		//
		// GET: /UserServices/Delete
		
		public ActionResult Delete(int ServiceId, int UserId)
		{
			return View(repository.All().Single(x => x.ServiceId == ServiceId && x.UserId == UserId));
		}
		
		//
		// POST: /UserServices/Delete
		
		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int ServiceId, int UserId)
		{
			using (IUnitOfWork uow = uowFactory.Create()) {
				repository.Remove(repository.All().Single(x => x.ServiceId == ServiceId && x.UserId == UserId));
				uow.Save();
				return RedirectToAction("Index");
			}
		}

		private static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(IQueryable<TSource> source, System.Linq.Expressions.Expression<Func<TSource, TKey>> keySelector, bool ascending) {

			return ascending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
		}
	}
}

