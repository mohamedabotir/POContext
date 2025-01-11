using System.ComponentModel;
using Domain.Result;

namespace Domain.ValueObject;

public class PoNumber:ValueObject<PoNumber>
{
    private PoNumber(string poNumberValue)
    {
        PoNumberValue = poNumberValue;
    }

    public static Result<PoNumber> CreateInstance(NumberGenerator numberGenerator)
    {
        NumberGeneratorBase numberGeneratorBase = NumberGeneratorBase.CreateGenerator(numberGenerator);
        var poNumber =(Maybe<string>) numberGeneratorBase.GenerateNumber();
        return poNumber.ToResult("PoNumber Cannot be null")
            .Ensure(e => e.Length > 10,"Po Must be Greater than 10")
            .Map(x=>new PoNumber(x));
    }

    private string PoNumberValue { get; }
    protected override bool EqualsCore(PoNumber other)
    {
        return other.PoNumberValue == PoNumberValue;
    }

    protected override int GetHashCodeCore()
    {
        return GetHashCode();
    }
    
}

public enum  NumberGenerator
{
    [Description("Po-yymmdd")]
    PoAndyymmdd,
    [Description("Po{timestamp}")]
    PoTimestamp
}

public abstract class NumberGeneratorBase
{
   public abstract string GenerateNumber();

   public static NumberGeneratorBase CreateGenerator(NumberGenerator generator) => generator switch
   {
       NumberGenerator.PoAndyymmdd => new PoNumberDateGenerator(),
       NumberGenerator.PoTimestamp => new PoNumberTimestampGenerator(),
       _ => throw new ArgumentOutOfRangeException(nameof(generator), generator, null)
   };
}
public class PoNumberDateGenerator : NumberGeneratorBase{
    public override string GenerateNumber()
    {
        var prefix = "PO";
        var date = DateTime.Now.ToString("yyyyMMdd");
        var randomCode = Guid.NewGuid().ToString("N").Substring(0, 5).ToUpper();
        return $"{prefix}-{date}-{randomCode}";
    }
}
public class PoNumberTimestampGenerator : NumberGeneratorBase{
    public override string GenerateNumber()
    {
        var prefix = "PO";
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        return $"{prefix}-{timestamp}";
    }
}