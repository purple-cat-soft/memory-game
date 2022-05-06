using Microsoft.AspNetCore.Components;

namespace MemoryGame.Pages;

public partial class Game
{
  [Parameter] public int Level { get; set; } = 1;
}
