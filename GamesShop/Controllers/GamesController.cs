using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GamesShop.Data;
using GamesShop.Models.Store;
using GamesShop.Models.Store.SroreViewModel;

namespace GamesShop.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationContext _context;

        public GamesController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Games.Include(g => g.Developer);
            return View(await applicationContext.ToListAsync());
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.Developer)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            if(selectedGenres != null)
            {
                game.GenreAssigment = new List<GenreAssigment>();
                foreach(var genre in selectedGenres)
                {
                    var genreToAdd = new GenreAssigment { GameId = game.Id ,GenreId = int.Parse(genre) };
                    game.GenreAssigment.Add(genreToAdd);
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
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

            var game = await _context.Games
                .Include(i => i.GenreAssigment).ThenInclude(i => i.Genre)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
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
                UpdateGameGenres(selectedGenres, gameToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(gameToUpdate.Id))
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
            UpdateGameGenres(selectedGenres, gameToUpdate);
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

            var game = await _context.Games
                .Include(g => g.Developer)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var game = await _context.Games.FindAsync(id);
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }

        private void UpdateGameGenres(string[] selectedGenres, Game gameToUpdate)
        {
            if (selectedGenres == null)
            {
                gameToUpdate.GenreAssigment = new List<GenreAssigment>();
                return;
            }

            var selectedGenresHS = new HashSet<string>(selectedGenres);
            var gameGenres = new HashSet<int>
                (gameToUpdate.GenreAssigment.Select(c => c.Genre.IdGenre));
            foreach (var genre in _context.Genres)
            {
                if (selectedGenresHS.Contains(genre.IdGenre.ToString()))
                {
                    if (!gameGenres.Contains(genre.IdGenre))
                    {
                        gameToUpdate.GenreAssigment.Add(new GenreAssigment { GameId = gameToUpdate.Id, GenreId = genre.IdGenre });
                    }
                }
                else
                {

                    if (gameGenres.Contains(genre.IdGenre))
                    {
                        GenreAssigment generToRemove = gameToUpdate.GenreAssigment.FirstOrDefault(i => i.GenreId == genre.IdGenre);
                        _context.Remove(generToRemove);
                    }
                }
            }
        }

        private void PopulateDeveloperDropDownList(object selectedDeveloper = null)
        {
            var developerQuery = from d in _context.Developers
                                   orderby d.Name
                                   select d;
            ViewBag.IdDeveloper = new SelectList(developerQuery, "IdDeveloper", "Name", selectedDeveloper);
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
