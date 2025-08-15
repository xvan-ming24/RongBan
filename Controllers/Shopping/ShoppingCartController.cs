using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ShoppingCartApi.BLL;
using ShoppingCartApi.Models.Dtos;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ShoppingCartApi.Controllers
{
    /// <summary>
    /// 购物车API控制器
    /// 提供RESTful风格的购物车操作接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingCartController : ControllerBase
    {
        // 购物车业务逻辑对象
        private readonly IShoppingCartService _cartService;

        /// <summary>
        /// 构造函数，通过依赖注入获取业务逻辑层对象
        /// </summary>
        public ShoppingCartController(IShoppingCartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// 获取用户购物车信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>购物车信息</returns>
        [HttpGet]
        public async Task<IActionResult> GetUserCart()
        {
            try
            {
                var currentUserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (currentUserId == null)
                {
                    return BadRequest("用户未登录");
                }
                var cart = await _cartService.GetUserShoppingCartAsync(currentUserId);
                return Ok(cart); // 返回200 OK及购物车数据
            }
            catch (Exception ex)
            {
                // 记录异常日志
                return StatusCode(500, $"获取购物车失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 添加商品到购物车
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="itemDto">购物车项信息</param>
        /// <returns>更新后的购物车信息</returns>
        [HttpPost("items")]
        public async Task<IActionResult> AddItemToCart([FromBody] CartItemDto itemDto)
        {
            // 验证模型状态
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 返回400 Bad Request及验证错误信息

            try
            {
                var currentUserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (currentUserId == null)
                {
                    return BadRequest("用户未登录");
                }

                var updatedCart = await _cartService.AddItemToCartAsync(currentUserId, itemDto);
                return Ok(updatedCart); // 返回200 OK及更新后的购物车数据
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // 返回400 Bad Request及错误信息
            }
            catch (Exception ex)
            {
                // 记录异常日志
                return StatusCode(500, $"添加商品到购物车失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 更新购物车商品数量
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="productId">商品ID</param>
        /// <param name="quantity">新数量</param>
        /// <returns>更新后的购物车信息</returns>
        [HttpPut("items/{productId}/quantity")]
        public async Task<IActionResult> UpdateItemQuantity( long productId, [FromBody] int quantity)
        {
            try
            {
                var currentUserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (currentUserId == null)
                {
                    return BadRequest("用户未登录");
                }
                var updatedCart = await _cartService.UpdateCartItemQuantityAsync(currentUserId, productId, quantity);
                return Ok(updatedCart); // 返回200 OK及更新后的购物车数据
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // 返回400 Bad Request及错误信息
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // 返回404 Not Found及错误信息
            }
            catch (Exception ex)
            {
                // 记录异常日志
                return StatusCode(500, $"更新商品数量失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 更新购物车商品选中状态
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="productId">商品ID</param>
        /// <param name="isSelected">选中状态：1-选中，0-未选中</param>
        /// <returns>更新后的购物车信息</returns>
        [HttpPut("items/{productId}/selection")]
        public async Task<IActionResult> UpdateItemSelection( long productId, [FromBody] byte isSelected)
        {
            try
            {
                var currentUserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (currentUserId == null)
                {
                    return BadRequest("用户未登录");
                }

                var updatedCart = await _cartService.UpdateCartItemSelectionAsync(currentUserId, productId, isSelected);
                return Ok(updatedCart); // 返回200 OK及更新后的购物车数据
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // 返回400 Bad Request及错误信息
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // 返回404 Not Found及错误信息
            }
            catch (Exception ex)
            {
                // 记录异常日志
                return StatusCode(500, $"更新商品选中状态失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 从购物车移除商品
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="productId">商品ID</param>
        /// <returns>更新后的购物车信息</returns>
        [HttpDelete("items/{productId}")]
        public async Task<IActionResult> RemoveItemFromCart( long productId)
        {
            try
            {
                var currentUserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (currentUserId == null)
                {
                    return BadRequest("用户未登录");
                }

                var updatedCart = await _cartService.RemoveItemFromCartAsync(currentUserId, productId);
                return Ok(updatedCart); // 返回200 OK及更新后的购物车数据
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message); // 返回404 Not Found及错误信息
            }
            catch (Exception ex)
            {
                // 记录异常日志
                return StatusCode(500, $"移除商品失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 清空购物车
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>无内容</returns>
        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var currentUserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (currentUserId == null)
                {
                    return BadRequest("用户未登录");
                }

                await _cartService.ClearShoppingCartAsync(currentUserId);
                return NoContent(); // 返回204 No Content表示操作成功且无返回内容
            }
            catch (Exception ex)
            {
                // 记录异常日志
                return StatusCode(500, $"清空购物车失败: {ex.Message}");
            }
        }
    }
}
