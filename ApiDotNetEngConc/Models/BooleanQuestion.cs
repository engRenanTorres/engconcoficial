namespace DotnetAPI.Models;

using DotnetAPI.Models.Inharitance;
using System.ComponentModel.DataAnnotations.Schema;

public class BooleanQuestion : BaseQuestion
{
    [NotMapped]
    private Choice _ChoiceA = new()
    {
        Letter = 'A',
        Text = "Verdadeiro"
    };
    [NotMapped]
    private Choice _ChoiceB = new()
    {
        Letter = 'B',
        Text = "Falso"
    };
    [NotMapped]

    public override ICollection<Choice> Choices
    {
        get { return new[] { _ChoiceA, _ChoiceB }; }
        set
        {
            if (value.First() != null) _ChoiceA = value.First();
            if (value.ElementAt(1) != null) _ChoiceB = value.ElementAt(1);
        }
    }
}