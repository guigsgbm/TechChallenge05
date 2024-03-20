using Domain;
using Infrastructure.DB.Repository;
using Infrastructure.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace ItemApi.Controllers;

[Route("api/items")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly ItemRepository _itemRepository;
    private readonly ItemMessaging _itemMessaging;

    public ItemController(ItemRepository itemRepository, ItemMessaging itemMessaging)
    {
        _itemRepository = itemRepository;
        _itemMessaging = itemMessaging;
    }

    [HttpPost]
    public async Task<IActionResult> UploadItemAsync([FromBody] SimplifiedItem item)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        //var response = await _itemRepository.Add(item);
        //await _itemRepository.Save();
        
        await _itemMessaging.SendMessageAsync(item);

        return Ok(item);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetItemByIdAsync(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var item = await _itemRepository.GetById(id);

        if (item == null)
            return NotFound("Item not found");

        return Ok(await _itemRepository.GetById(id));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllItemsAsync([FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        var items = await _itemRepository.GetAll(skip, take);

        if (!items.Any())
            return NotFound("Items aren't found");

        return Ok(items);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItemByIdAsync(int id)
    {

        if (await _itemRepository.DeleteById(id) == null) 
            return NotFound();

        await _itemRepository.Save();
        return Ok();
    }

}
 