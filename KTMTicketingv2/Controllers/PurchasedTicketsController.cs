using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using KTMTicketingv2.Models;

namespace KTMTicketingv2.Controllers
{
    public class PurchasedTicketsController : Controller
    {
        private KTMTicketingEntities db = new KTMTicketingEntities();

        // GET: PurchasedTickets
        public async Task<ActionResult> Index()
        {
            return View(await db.PurchasedTickets.ToListAsync());
        }

        // GET: PurchasedTickets/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchasedTicket purchasedTicket = await db.PurchasedTickets.FindAsync(id);
            if (purchasedTicket == null)
            {
                return HttpNotFound();
            }
            return View(purchasedTicket);
        }

        // GET: PurchasedTickets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PurchasedTickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TicketID,EmailID,Origin,Destin,Price,PurchasedOn,Quantity,TicketType")] PurchasedTicket purchasedTicket)
        {
            if (ModelState.IsValid)
            {
                db.PurchasedTickets.Add(purchasedTicket);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(purchasedTicket);
        }

        // GET: PurchasedTickets/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchasedTicket purchasedTicket = await db.PurchasedTickets.FindAsync(id);
            if (purchasedTicket == null)
            {
                return HttpNotFound();
            }
            return View(purchasedTicket);
        }

        // POST: PurchasedTickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TicketID,EmailID,Origin,Destin,Price,PurchasedOn,Quantity,TicketType")] PurchasedTicket purchasedTicket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchasedTicket).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(purchasedTicket);
        }

        // GET: PurchasedTickets/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchasedTicket purchasedTicket = await db.PurchasedTickets.FindAsync(id);
            if (purchasedTicket == null)
            {
                return HttpNotFound();
            }
            return View(purchasedTicket);
        }

        // POST: PurchasedTickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PurchasedTicket purchasedTicket = await db.PurchasedTickets.FindAsync(id);
            db.PurchasedTickets.Remove(purchasedTicket);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
