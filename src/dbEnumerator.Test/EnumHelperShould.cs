using System;
using System.ComponentModel;
using FluentAssertions;
using Xunit;

namespace dbEnumerator.Test
{
    public class EnumHelperShould
    {
        [Fact]
        public void ensure_that_a_type_is_enum()
        {
            var type = EnumHelper.EnsureEnum<ValidEnum>();
            type.Should().Be(typeof(ValidEnum));
        }

        [Fact]
        public void detect_a_type_that_is_not_an_enum()
        {
            Action act = () => EnumHelper.EnsureEnum<int>();

            act
                .Should().Throw<ArgumentException>()
                .WithMessage("Type * must be an enum");
        }

        [Fact]
        public void detect_an_enum_not_based_on_int()
        {
            Action act = () => EnumHelper.EnsureEnum<UIntBasedEnum>();

            act
                .Should().Throw<ArgumentException>()
                .WithMessage("Underlying enum type must be int");
        }

        [Fact]
        public void get_enum_description()
        {
            EnumHelper.GetEnumDescription(ValidEnum.A)
                .Should().Be("A description");
        }

        [Fact]
        public void get_empty_description_if_here_si_no_description()
        {
            EnumHelper.GetEnumDescription(ValidEnum.B)
                .Should().BeEmpty();
        }
    }

    enum ValidEnum
    {
        [Description("A description")]
        A=1,
        B=2
    }

    enum InvalidValidEnum
    {
        [Description("A description")]
        A = 0,
        B = 1
    }

    enum UIntBasedEnum: uint
    {
        A = 1,
        B = 2
    }
}
