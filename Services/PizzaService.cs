using aspnetcore_with_reactspa.Models;
using aspnetcore_with_reactspa.Data;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore_with_reactspa.Services;

public class PizzaService
{
    private readonly PizzaContext _context;

    public PizzaService(PizzaContext context)
    {
        _context = context;
    }

    public IEnumerable<Pizza> GetAll()
    {
        return _context.Pizzas
         .Include(p => p.Toppings)
         .Include(p => p.Sauce)
         .AsNoTracking()
         .ToList();

    }
    public IEnumerable<Sauce> GetAllSauces()
    {
        return _context.Sauces
         .AsNoTracking()
         .ToList();
    }

    public IEnumerable<Topping> GetAllToppings()
    {
        return _context.Toppings
         .AsNoTracking()
         .ToList();
    }

    public Pizza? GetById(int id)
    {
        return _context.Pizzas
           .Include(p => p.Toppings)
           .Include(p => p.Sauce)
           .AsNoTracking()
           .SingleOrDefault(p => p.Id == id);

    }
    public Pizza? GetByIdT(int id)
    {
        return _context.Pizzas
           .Include(p => p.Toppings)
           .Include(p => p.Sauce)
           .SingleOrDefault(p => p.Id == id);

    }

    // public Pizza? Create(Pizza newPizza)
    // {
    //     _context.Pizzas.Add(newPizza);
    //     _context.SaveChanges();

    //     return newPizza;
    // }

    public Pizza? Create(PizzaRequest newPizza)
    {
        var pizza = new Pizza
        {
            Id = newPizza.id,
            Name = newPizza.name
        };

        _context.Pizzas.Add(pizza);

        _context.SaveChanges();

        if (newPizza.sauce != null)
            UpdateSauce(pizza.Id, newPizza.sauce.GetValueOrDefault(1));

        if (newPizza.toppings != null)
            foreach (var topping in newPizza.toppings)
            {
                AddTopping(pizza.Id, topping);
            }


        return pizza;
    }

    public void AddTopping(int PizzaId, int ToppingId)
    {
        var pizzaToUpdate = _context.Pizzas.Find(PizzaId);
        var toppingToAdd = _context.Toppings.Find(ToppingId);

        if (pizzaToUpdate is null || toppingToAdd is null)
        {
            throw new NullReferenceException("Pizza or topping does not exist");
        }

        if (pizzaToUpdate.Toppings is null)
        {
            pizzaToUpdate.Toppings = new List<Topping>();
        }

        pizzaToUpdate.Toppings.Add(toppingToAdd);

        _context.Pizzas.Update(pizzaToUpdate);
        _context.SaveChanges();

    }

    public void UpdatePizza(PizzaRequest pizza)
    {
        var pizzaToUpdate = GetByIdT(pizza.id);
        if (pizzaToUpdate == null) return;
        if (pizza.name != null)
        {
            pizzaToUpdate.Name = pizza.name;
        }
        if (pizza.sauce != null)
        {
            var sauceToUpdate = _context.Sauces.Find(pizza.sauce);
            pizzaToUpdate.Sauce = sauceToUpdate;
        }
        if (pizza.toppings != null)
        {
            pizzaToUpdate.Toppings = new List<Topping>();
            foreach (var toppingID in pizza.toppings)
            {
                var toppingToAdd = _context.Toppings.Find(toppingID);
                if (toppingToAdd != null)
                    pizzaToUpdate.Toppings.Add(toppingToAdd);
            }
        }
        _context.Pizzas.Update(pizzaToUpdate);
        _context.SaveChanges();
    }
    public void UpdateSauce(int PizzaId, int SauceId)
    {
        var pizzaToUpdate = _context.Pizzas.Find(PizzaId);
        var sauceToUpdate = _context.Sauces.Find(SauceId);

        if (pizzaToUpdate is null || sauceToUpdate is null)
        {
            throw new NullReferenceException("Pizza or sauce         does not exist");
        }

        pizzaToUpdate.Sauce = sauceToUpdate;

        _context.SaveChanges();

    }

    public void UpdateName(int PizzaId, string PizzaName)
    {
        var pizzaToUpdate = _context.Pizzas.Find(PizzaId);

        if (pizzaToUpdate is null)
        {
            throw new NullReferenceException("Pizza or sauce         does not exist");
        }

        pizzaToUpdate.Name = PizzaName;

        _context.SaveChanges();

    }

    public void DeleteById(int id)
    {
        var pizzaDel = _context.Pizzas.Find(id);
        if (pizzaDel is null)
        {
            throw new NullReferenceException("Pizza does not exist");
        }
        _context.Pizzas.Remove(pizzaDel);
        _context.SaveChanges();
    }
}