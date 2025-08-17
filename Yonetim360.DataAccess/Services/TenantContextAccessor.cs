using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yonetim360.DataAccess.Services
{
    public class TenantContextAccessor : ITenantContextAccessor
    {
        private static readonly AsyncLocal<Guid?> _current = new();
        public Guid? CurrentTenantId => _current.Value;

        public IDisposable Use(Guid TenantId)
        {
            var previous = _current.Value;
            _current.Value = TenantId;
            return new Restore(() => _current.Value = previous);
        }
        private sealed class Restore : IDisposable { private readonly Action _a; public Restore(Action a) => _a = a; public void Dispose() => _a(); }
    }
}
