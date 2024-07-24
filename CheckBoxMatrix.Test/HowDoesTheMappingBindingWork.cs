using Avalonia;
using Avalonia.Data;
using CheckBoxMatrix.Control;
using CheckBoxMatrix.Demo;
using FluentAssertions;

namespace CheckBoxMatrix.Test;

public class HowDoesTheMappingBindingWork
{
    [Fact]
    public void OneWayToSource()
    {
        var control = new Control.CheckBoxMatrix();
        var vm = new MyViewModel();
        var binding = new Binding { Source = vm, Path = nameof(vm.Mappings), Mode = BindingMode.OneWayToSource };
        control.Bind(Control.CheckBoxMatrix.MappingsProperty, binding);

        control.Mappings.Should().BeEmpty();
        vm.Mappings.Should().BeEmpty();

        control.Mappings = [("a", "1")];
        control.Mappings.Should().Contain([("a", "1")]);
        vm.Mappings.Should().Contain([("a", "1")]);

        vm.Mappings = [];
        vm.Mappings.Should()
            .NotBeEmpty("because the OneWayToSource binding interferes")
            .And.Contain([("a", "1")])
            .And.HaveCount(1);
        control.Mappings.Should()
            .Contain([("a", "1")])
            .And.HaveCount(1);
    }
}
