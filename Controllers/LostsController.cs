using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Zadanko3.Models;

namespace Zadanko3.Controllers
{
    public class LostsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        IHostingEnvironment _env;
        public LostsController(DataContext context, IHostingEnvironment environment,UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
            _env = environment;
        }
        public async Task<IActionResult> MyLost()
        {   
            if(User.Identity.Name=="Admin")
            {
                return View(await _context.Losts.ToListAsync());
            }
            return View(await _context.Losts.Where(model=>model.NazwaUser==User.Identity.Name.ToString()).ToListAsync());
        }
        
        public async Task<IActionResult> Index(Lost.WojewództwoList województwolist,Lost.PlecList pleclist)
        {
            IEnumerable<Lost> losts = await _context.Losts.ToListAsync();
            if(województwolist!=Lost.WojewództwoList.Wszystkie && pleclist!=Lost.PlecList.Brak)
            {
                losts = await _context.Losts.Where(model => model.WojewództwoLista == województwolist && model.Plec == pleclist).ToListAsync();
                return View(losts);
            }
            if (województwolist != Lost.WojewództwoList.Wszystkie)
            {
                var wojewodztwa = await _context.Losts.Where(model => model.WojewództwoLista == województwolist).ToListAsync();

                
                return View(wojewodztwa);
            }
            if (pleclist != Lost.PlecList.Brak)
            {
                var sex = await _context.Losts.Where(model => model.Plec == pleclist).ToListAsync();
                
                return View(sex);
            }
            return View(losts);
        }

        
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lost = await _context.Losts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lost == null)
            {
                return NotFound();
                
            }

            return View(lost);
        }

        
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Imie,Nazwisko,Wiek,Plec,Wzrost,Miejscowość,Ulica,NumerKontaktowy,WojewództwoLista,Opis")] Lost lost, IFormFile file)
        {
            if (ModelState.IsValid)

            {
                if (file != null && file.Length > 0)
                {
                    
                    var imagePath = @"\images\";
                    var uploadPath = _env.WebRootPath + imagePath;
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    var uniqFileName = Guid.NewGuid().ToString();
                    var filename = Path.GetFileName(uniqFileName + "." + file.FileName.Split(".")[1].ToLower());
                    string fullpath = uploadPath + filename;
                    imagePath = imagePath + @"\";
                    var filePath = @".." + Path.Combine(imagePath, filename);
                    using (var fileStream = new FileStream(fullpath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    ViewData["FileLocation"] = filePath;
                    lost.Img = filename;
                }

                if (ModelState.IsValid)
                {
                    lost.NazwaUser = User.Identity.Name;
                    lost.Id = Guid.NewGuid();
                    _context.Add(lost);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(MyLost));
                }
                return View(lost);
            }
            return View(lost);
        }

       
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lost = await _context.Losts.FindAsync(id);
            if (lost == null)
            {
                return NotFound();
            }
            return View(lost);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Imie,Nazwisko,Wiek,Plec,Wzrost,Miejscowość,Ulica,NumerKontaktowy,Województwo,Img")] Lost lost)
        {
            if (id != lost.Id)
            {
                return NotFound();
                
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LostExists(lost.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MyLost));
            }
            return View(lost);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lost = await _context.Losts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lost == null)
            {
                return NotFound();
            }

            return View(lost);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var lost = await _context.Losts.FindAsync(id);
            if (lost.Img != null)
            {
                var imagePath = @"\images\";
                var uploadPath = _env.WebRootPath + imagePath;
                System.IO.File.Delete(uploadPath + lost.Img);
            }
            else
            _context.Losts.Remove(lost);
            await _context.SaveChangesAsync();
           
           
            return RedirectToAction(nameof(MyLost));
        }

        private bool LostExists(Guid id)
        {
            return _context.Losts.Any(e => e.Id == id);
        }
    }
}
