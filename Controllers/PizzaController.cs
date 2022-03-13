using aspnetcore_with_reactspa.Services;
using aspnetcore_with_reactspa.Models;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore_with_reactspa.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PizzaController : ControllerBase
{
    PizzaService _service;

    public PizzaController(PizzaService service)
    {
        _service = service;
    }

    [HttpGet]
    public IEnumerable<Pizza> GetAll()
    {
        return _service.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<Pizza> GetById(int id)
    {
        var pizza = _service.GetById(id);

        if (pizza is not null)
        {
            return pizza;
        }
        else
        {
            return NotFound();
        }
    }
    [HttpGet("sauces")]
    public IEnumerable<Sauce> GetSauces()
    {
        return _service.GetAllSauces();
    }

    [HttpGet("toppings")]
    public IEnumerable<Topping> GetToppings()
    {
        return _service.GetAllToppings();
    }

    

    [HttpPost]
    public IActionResult Create(PizzaA newPizza)
    {
        var pizza = _service.Create(newPizza);
        return CreatedAtAction(nameof(GetById), new { id = pizza!.Id }, pizza);
    }
    [HttpPost("updatePizza")]
    public IActionResult UpdatePizza(PizzaA pizza)
    {
        var pizzaToUpdate = _service.GetById(pizza.id);

        if (pizzaToUpdate is not null && pizza.name is not null)
        {
            _service.UpdatePizza(pizza);
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPut("{id}/addtopping")]
    public IActionResult AddTopping(int id, int toppingId)
    {
        var pizzaToUpdate = _service.GetById(id);

        if (pizzaToUpdate is not null)
        {
            _service.AddTopping(id, toppingId);
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpPut("{id}/updatesauce")]
    public IActionResult UpdateSauce(int id, int sauceId)
    {
        var pizzaToUpdate = _service.GetById(id);

        if (pizzaToUpdate is not null)
        {
            _service.UpdateSauce(id, sauceId);
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var pizza = _service.GetById(id);

        if (pizza is not null)
        {
            _service.DeleteById(id);
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}