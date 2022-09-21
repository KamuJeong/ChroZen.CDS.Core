using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromassProtocols.APIs
{
    public class UnionActionsAfter100ms
    {
        private Action? action;
        private int mergeCounts; 

        public UnionActionsAfter100ms(Action? action)
        {
            this.action = action;
        }

        public bool IsActive => action != null;

        public async void CallAction()
        {
            mergeCounts++;
            await Task.Delay(100);
            if(--mergeCounts == 0)
            {
                action?.Invoke();
            }
        }
    }
}
