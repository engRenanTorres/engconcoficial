namespace DotnetAPI.Models;

using DotnetAPI.Models.Inharitance;

public class MultipleChoicesQuestion : BaseQuestion
{
    private Choice? _ChoiceA { get; set; }
    private Choice? _ChoiceB { get; set; }

    private Choice? _ChoiceC { get; set; }

    public override ICollection<Choice> Choices
    {
        get => new[] { _ChoiceA ?? new Choice(), _ChoiceB ?? new Choice(), _ChoiceC ?? new Choice() };
        set
        {
            if (value.First() != null) _ChoiceA = value.First();
            if (value.ElementAt(1) != null) _ChoiceB = value.ElementAt(1);
            if (value.ElementAt(2) != null) _ChoiceC = value.ElementAt(2);
        }
    }
}
