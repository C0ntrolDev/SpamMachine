using System;
using SpamBotRemaster.Infrastructure.Commands.Base;

namespace SpamBotRemaster.Infrastructure.Commands
{
    internal class LambdaCommand : Command
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool>? canExecute;

        public LambdaCommand(Action<object> execute, Func<object, bool>? canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        public override bool CanExecute(object parameter) => canExecute == null || canExecute.Invoke(parameter);

        public override void Execute(object parameter) => execute.Invoke(parameter);

    }
}

