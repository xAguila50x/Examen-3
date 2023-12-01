using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TiendaWep.Data;
using TiendaWep.Models;

namespace TiendaWep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CervezasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

       public CervezasController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Admin/Cervezas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Cervezas.Include(c => c.Estilo);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/Cervezas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cerveza = await _context.Cervezas
                .Include(c => c.Estilo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cerveza == null)
            {
                return NotFound();
            }

            return View(cerveza);
        }

        // GET: Admin/Cervezas/Create
        public IActionResult Create()
        {
            ViewData["idEstilo"] = new SelectList(_context.Estilos, "Id", "nombre");
            return View();
        }

        // POST: Admin/Cervezas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,nombre,alcohol,idEstilo,Precio,UrlImagen")] Cerveza cerveza)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostEnvironment.WebRootPath;
                var archivos=HttpContext.Request.Form.Files;
                if(archivos.Count > 0){
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\cervezas");
                    var extesion = Path.GetExtension(archivos[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(subidas, nombreArchivo + extesion), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStream);
                    }
                    cerveza.UrlImagen = @"imagenes\cervezas\"+nombreArchivo+extesion;
                }
                _context.Add(cerveza);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["idEstilo"] = new SelectList(_context.Estilos, "Id", "nombre", cerveza.idEstilo);
            return View(cerveza);
        }

        // GET: Admin/Cervezas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cerveza = await _context.Cervezas.FindAsync(id);
            if (cerveza == null)
            {
                return NotFound();
            }
            ViewData["idEstilo"] = new SelectList(_context.Estilos, "Id", "nombre", cerveza.idEstilo);
            return View(cerveza);
        }

        // POST: Admin/Cervezas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,nombre,alcohol,idEstilo,Precio,UrlImagen")] Cerveza cerveza)
        {
            if (id != cerveza.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string rutaPrincipal = _hostEnvironment.WebRootPath;
                    var archivos = HttpContext.Request.Form.Files;
                    if(archivos.Count >0)
                    {
                        Cerveza? cervezabd = await _context.Cervezas.FindAsync(id);
                        if(cervezabd != null && cervezabd.UrlImagen != null)
                        {
                            var rutaImagenActual = Path.Combine(rutaPrincipal, cervezabd.UrlImagen);
                            if (System.IO.File.Exists(rutaImagenActual))
                            {
                                System.IO.File.Delete(rutaImagenActual);
                            }
                           
                        }
                        _context.Entry(cervezabd).State = EntityState.Detached;
                        string nombreArchivo = Guid.NewGuid().ToString();
                        var subidas = Path.Combine(rutaPrincipal, @"imagenes\cervezas");
                        var extesion = Path.GetExtension(archivos[0].FileName);
                        using (var fileStream = new FileStream(Path.Combine(subidas, nombreArchivo + extesion), FileMode.Create))
                        {
                            archivos[0].CopyTo(fileStream);
                        }
                        cerveza.UrlImagen = @"imagenes\cervezas\" + nombreArchivo + extesion;
                        _context.Entry(cerveza).State = EntityState.Modified;

                    }
                    _context.Update(cerveza);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CervezaExists(cerveza.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["idEstilo"] = new SelectList(_context.Estilos, "Id", "nombre", cerveza.idEstilo);
            return View(cerveza);
        }

        // GET: Admin/Cervezas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cerveza = await _context.Cervezas
                .Include(c => c.Estilo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cerveza == null)
            {
                return NotFound();
            }

            return View(cerveza);
        }

        // POST: Admin/Cervezas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cerveza = await _context.Cervezas.FindAsync(id);
            if (cerveza != null)
            {
                _context.Cervezas.Remove(cerveza);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CervezaExists(int id)
        {
            return _context.Cervezas.Any(e => e.Id == id);
        }
    }
}
