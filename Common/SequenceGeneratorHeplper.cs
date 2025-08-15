

using Microsoft.EntityFrameworkCore;
using Rongban.Models.Entities;

namespace YourProject.Utilities;


public class SequenceGeneratorHeplper
{
    private readonly PetPlatformDbContext _dbContext;

    public SequenceGeneratorHeplper(PetPlatformDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<string> GetNextSequenceAsync(string sequenceName)
    {
        if (string.IsNullOrWhiteSpace(sequenceName))
            throw new ArgumentException("序列名称不能为空", nameof(sequenceName));

        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {
            var counter = await _dbContext.SequenceCounters
                .FromSqlInterpolated($"SELECT * FROM SequenceCounter WITH (UPDLOCK, HOLDLOCK) WHERE SequenceName = {sequenceName}")
                .FirstOrDefaultAsync();

            if (counter == null)
                throw new KeyNotFoundException($"未找到序列：{sequenceName}");

            // 递增计数器（从10000000 → 10000001开始）
            counter.CurrentValue++;
            var currentValue = counter.CurrentValue;

            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            // 计算需要的位数（从8位开始，满后自动扩展）
            int digits = GetRequiredDigits(currentValue);

            // 格式化输出（确保位数不足时补零，这里从8位开始不需要补零，因为起始值已满足8位）
            return currentValue.ToString();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <summary>
    /// 从8位数开始，满后自动扩展（如99999999后变为100000000）
    /// </summary>
    private int GetRequiredDigits(long value)
    {
        // 由于起始值是10000001（8位），且按整数递增，无需补零，直接返回实际位数即可
        // 这里保留逻辑用于后续可能的扩展
        return value.ToString().Length;
    }
}
