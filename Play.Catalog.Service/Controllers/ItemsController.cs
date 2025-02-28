using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController  : ControllerBase
    {
        private static readonly List<ItemDto> items = new(){
            new ItemDto(Guid.NewGuid(), "Potion", "Restore a small amount of HP", 5, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(),"Antidote", "Cure poison", 7, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Bronze Sword", "Deal some damage", 20, DateTimeOffset.UtcNow)
        };

        [HttpGet]
        public IEnumerable<ItemDto> GetItems(){
            return items;
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id){
            var item = items.Where(item => item.Id == id).SingleOrDefault();
            if(item is null){
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public ActionResult<ItemDto> Post(CreateItemDto createItemDto) 
        {
            var newItem = new ItemDto(Guid.NewGuid(), createItemDto.Name, createItemDto.Description, createItemDto.Price, DateTimeOffset.UtcNow);
            items.Add(newItem);
            return CreatedAtAction(nameof(GetItem), new { id = newItem.Id }, newItem);
        }

        [HttpPut("{id}")]
        public ActionResult Put(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = items.Where(item => item.Id == id).SingleOrDefault();
            if(existingItem is null){
                return NotFound();
            }
            var updatedItem = existingItem with {
                Name = updateItemDto.Name,
                Description = updateItemDto.Description,
                Price = updateItemDto.Price
            };
            var index = items.FindIndex(existingItem => existingItem.Id == id);
            items[index] = updatedItem;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(Guid id)
        {
            var existingItem = items.Where(item => item.Id == id).SingleOrDefault();
            if(existingItem is null){
                return NotFound();
            }
            items.Remove(existingItem);
            return NoContent();
        }
    }
}