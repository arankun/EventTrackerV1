using EventTracker.DataModel.UnitOfWork;
using EventTracker.Resolver;
using System.ComponentModel.Composition;
namespace EventTracker.DataModel
{
    [Export(typeof(IComponent))]
    public class DependencyResolver : IComponent
    {
        public void SetUp(IRegisterComponent registerComponent)
        {
            registerComponent.RegisterType<IUnitOfWork, UnitOfWork.UnitOfWork>();
        }
    }
}