#region directives

using System.ComponentModel.Composition;
using EventTracker.DataModel.UnitOfWork;
using EventTracker.Resolver;

#endregion

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