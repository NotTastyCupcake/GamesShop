using GamesShop.Data;
using GamesShop.Models.Store;
using GamesShop.Models.Store.SroreViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesShop.Controllers
{
    public class GamesController : Controller
    {
        private readonly IRepositoryGame _db;
        private readonly ApplicationContext _context;

        public GamesController(ApplicationContext context)
        {
            this._db = new GameReposiroty(context);
            this._context = context;
        }

        // GET: Games
        public async Task<IActionResult> Index(string gameName, int? idDeveloper, int? idGenre )
        {
            var games = _db.SearchItems(gameName,idDeveloper,idGenre);
            PopulateDeveloperDropDownList();
            PopulateGenreDropDownList();
            return View(await games.ToListAsync());
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _db.GetItemAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            var game = new Game();
            game.GenreAssigment = new List<GenreAssigment>();
            PopulateAssignedGenreData(game);
            PopulateDeveloperDropDownList();
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,IdDeveloper")] Game game, string[] selectedGenres)
        {
            _db.Create(game,selectedGenres);
            if (ModelState.IsValid)
            {
                _db.Create(game);
                await _db.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateAssignedGenreData(game);
            PopulateDeveloperDropDownList(game.IdDeveloper);
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _db.GetItemAsync(id);

            if (game == null)
            {
                return NotFound();
            }
            PopulateDeveloperDropDownList(game.IdDeveloper);
            PopulateAssignedGenreData(game);
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedGenres)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameToUpdate = await _context.Games
                 .Include(i => i.GenreAssigment)
                 .ThenInclude(i => i.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (await TryUpdateModelAsync<Game>(
              gameToUpdate,
              "",
              i => i.Name, i => i.IdDeveloper))
            {
                _db.Update(gameToUpdate, selectedGenres);
                try
                {
                    await _db.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_db.ItemExist(gameToUpdate.Id))
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
            _db.Update(gameToUpdate, selectedGenres);
            PopulateDeveloperDropDownList(gameToUpdate.IdDeveloper);
            return View(gameToUpdate);
        }

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var game = await _db.GetItemAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _db.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private void PopulateDeveloperDropDownList(object selectedDeveloper = null)
        {
            var developerQuery = from d in _context.Developers
                                 orderby d.Name
                                 select d;
            ViewBag.IdDeveloper = new SelectList(developerQuery, "IdDeveloper", "Name", selectedDeveloper);
        }
        private void PopulateGenreDropDownList(object selectedGenre = null)
        {
            var genreQuery = from d in _context.Genres
                                 orderby d.Name
                                 select d;
            ViewBag.IdGenre = new SelectList(genreQuery, "IdGenre", "Name", selectedGenre);
        }
        private void PopulateAssignedGenreData(Game game)
        {
            var allGenres = _context.Genres;
            var gameGenres = new HashSet<int>(game.GenreAssigment.Select(c => c.GenreId));
            var viewModel = new List<AssignedGenreData>();
            foreach (var genres in allGenres)
            {
                viewModel.Add(new AssignedGenreData
                {
                    IdGenre = genres.IdGenre,
                    Name = genres.Name,
                    Assigned = gameGenres.Contains(genres.IdGenre)
                });
            }
            ViewData["Genres"] = viewModel;
        }
    }
}
