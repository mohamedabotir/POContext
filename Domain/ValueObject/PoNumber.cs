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
                return new Result<PoNumber>(new PoNumber(poNumberValue),true,string.Empty);
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
    private readonly PoNumberPart[] _generatorPart =
    [
        new PoNumberPart(1, "PO",2),
        new PoNumberPart(2, "-",1),
        new PoNumberPart(3, $"{DateTime.Now:yyyyMMdd}",8),
        new PoNumberPart(4, "-",1),
        new PoNumberPart(5,"aaaaa",5),
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
            if (part.OrderPart == 3)
            {
                var substring = number.Substring(currentIndex, part.Length);
                if (DateTime.TryParse(substring, out _))
                {
                    return false;
                }
                currentIndex += part.Length;
            }
            else
            {
                var substring = number.Substring(currentIndex, part.Length);
                if (substring.Length != part.PartValue.Length)
                {
                    return false;
                }
                currentIndex += part.Length;
            }
        }

        return currentIndex == number.Length;
    }
}
public class PoNumberTimestampGenerator : NumberGeneratorBase
{

    private readonly PoNumberPart[] _generatorPart =
    [
        new PoNumberPart(1, "PO",2),
        new PoNumberPart(2, "{",1),
        new PoNumberPart(3, $"{DateTime.Now:yyyyMMdd-HHmmss}",8),
        new PoNumberPart(4, "}",1),
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
            if (part.OrderPart == 3)
            {
                var substring = number.Substring(currentIndex, part.Length);
                if (DateTime.TryParse(substring, out _))
                {
                    return false;
                }
                currentIndex += part.Length;
            }
            else
            {
                var substring = number.Substring(currentIndex, part.Length);
                if (substring != part.PartValue)
                {
                    return false;
                }
                currentIndex += part.Length;
            }
        }

        return currentIndex == number.Length;
    }
}
public record  PoNumberPart(int OrderPart,string PartValue,int Length);