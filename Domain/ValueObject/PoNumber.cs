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

    public static Result<PoNumber> SetPoNumber(string poNumberValue)
    {
        var number = Result.Result.Fail<PoNumber>("Invalid PoNumber");
        foreach (int enumValue in Enum.GetValues(typeof(NumberGenerator)))
        {
            var generator = NumberGeneratorBase.CreateGenerator((NumberGenerator)enumValue);
            if(generator.IsValidNumber(poNumberValue)) 
                number = new Result<PoNumber>(new PoNumber(poNumberValue),true,string.Empty);
        }
        return number;  
    }
    public string PoNumberValue { get; }
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
   public abstract bool IsValidNumber(string number);
}
public class PoNumberDateGenerator : NumberGeneratorBase{
    public override string GenerateNumber()
    {
        var prefix = "PO";
        var date = DateTime.Now.ToString("yyyyMMdd");
        var randomCode = Guid.NewGuid().ToString("N").Substring(0, 5).ToUpper();
        return $"{prefix}-{date}-{randomCode}";
    }

    public override bool IsValidNumber(string number)
    {
        throw new NotImplementedException();
    }
}
public class PoNumberTimestampGenerator : NumberGeneratorBase
{

    private readonly PoNumberPart[] _generatorPart =
    [
        new PoNumberPart(1, "Po"),
        new PoNumberPart(2, "{"),
        new PoNumberPart(3, $"{DateTime.Now:yyyyMMdd-HHmmss}"),
        new PoNumberPart(4, "}")
    ];
    
    public override string GenerateNumber()
    {
        var number = string.Concat(_generatorPart.OrderBy(e=>e.OrderPart)
            .Select(e=>e.PartValue));
        return number;
    }

    public override bool IsValidNumber(string number)
    {
        int currentIndex = 0;
        foreach (var part in _generatorPart.OrderBy(e => e.OrderPart))
        {
            int partIndex = number.IndexOf(part.PartValue, currentIndex, StringComparison.Ordinal);

            if (partIndex == -1 || partIndex < currentIndex)
            {
                return false;
            }
            currentIndex = partIndex + part.PartValue.Length;
        }
        return true;
    }
}
public record  PoNumberPart(int OrderPart,string PartValue);