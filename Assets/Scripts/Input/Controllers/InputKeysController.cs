public class InputKeysController : IUpdate
{
    #region Observers

    private InputKeyInstruction[] _instructions;

    #endregion

    #region Constructors

    public InputKeysController(
        InputKeyInstruction firstInstruction,
        params InputKeyInstruction[] otherInstructions)
    {
        _instructions = new InputKeyInstruction[1 + otherInstructions.Length];

        _instructions[0] = firstInstruction;

        for(int i = 0; i < otherInstructions.Length; i++)
        {
            _instructions[i + 1] = otherInstructions[i];
        };
    }

    #endregion

    #region Interface methods

    public void OnUpdate(float deltaTime)
    {
        for(int i = 0; i < _instructions.Length; i++)
        {
            _instructions[i].CheckInput();
        };
    }

    #endregion
}
