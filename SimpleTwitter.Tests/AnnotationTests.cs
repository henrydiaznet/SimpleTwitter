using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using SimpleTwitter.Api.Validation;

namespace SimpleTwitter.Tests;

public class AnnotationTests
{
    [Fact]
    public void Annotation_Validate_ReturnsError()
    {
        //arrange
        var test = new AnnotationTest
        {
            Property = null,
            LengthyProperty = "12345"
        };
        
        //act
        var actual = Annotations.Validate(test).ToList();

        //assert
        actual.Count().Should().Be(2);
        actual.Any(a => a.ErrorMessage == "Required").Should().BeTrue();
        actual.Any(a => a.ErrorMessage == "Max length 3").Should().BeTrue();
    }
    
    [Fact]
    public void Annotation_Validate_ReturnsOk()
    {
        //arrange
        var test = new AnnotationTest
        {
            Property = "prop",
            LengthyProperty = "123"
        };
        
        //act
        var actual = Annotations.Validate(test).ToList();

        //assert
        actual.Count().Should().Be(0);
    }

    private class AnnotationTest
    {
        [Required(ErrorMessage = "Required")]
        public string Property { get; set; }

        [Required(ErrorMessage = "Required")]
        [MaxLength(3, ErrorMessage = "Max length 3")]
        public string LengthyProperty { get; set; }
    }
}