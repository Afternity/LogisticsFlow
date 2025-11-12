using CommunityToolkit.Mvvm.ComponentModel;

namespace LogisticsFlow.ContractWPF.Common.ContractModels.MessageVMs
{
    public partial class MessageVm 
        : ObservableObject
    {
        [ObservableProperty]
        private string _message = string.Empty;

        [ObservableProperty]
        private MessageState _state = MessageState.None;

        public string SetMassage(
            MessageState state,
            string message)
        {
            Message = message;
            State = state;

            return State switch
            {
                MessageState.None => $"None: {Message}",
                MessageState.Info => $"Info: {Message}",
                MessageState.Warning => $"Warning: {Message}",
                MessageState.Error => $"Error: {Message}",
                MessageState.Success => $"Success: {Message}",
                MessageState.Fail => $"Fail: {Message}",
                _ => $"Unknown state ({(int)State}): {Message}"
            };
        }
    }
}
