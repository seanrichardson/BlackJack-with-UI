namespace BlackJackWPF
{
  using System;
  using System.Collections.Generic;
  using System.Collections.ObjectModel;
  using System.ComponentModel;
  using System.Linq;
  using System.Runtime.CompilerServices;
  using System.Text;
  using System.Threading.Tasks;

  /// <summary>
  /// A class that holds multiple card objects
  /// </summary>
  public class DeckVM : INotifyPropertyChanged
  {
    /// <summary>
    /// A collection of cards that are available to be dealt out
    /// </summary>
    private ObservableCollection<CardVM> listOfCards;

    /// <summary>
    /// An array of all possible card values
    /// </summary>
    private string[] cardValue = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };

    /// <summary>
    /// An array of all possible Suit values
    /// </summary>
    private Suit[] suit = { Suit.clubs, Suit.diamonds, Suit.hearts, Suit.spades };

    /// <summary>
    /// A collection of cards that have been dealt to players
    /// </summary>
    private ObservableCollection<int> usedCards;

    /// <summary>
    /// Constuctor for DeckVM initializes listOfCards and used Cards
    /// </summary>
    public DeckVM()
    {
      this.listOfCards = new ObservableCollection<CardVM>();
      this.usedCards = new ObservableCollection<int>();
    }

    /// <summary>
    /// Clears the used cards
    /// </summary>
    public void ClearUsedCards()
    {
      this.usedCards.Clear();
    }

    /// <summary>
    /// Creates a new Deck to be used for the game
    /// </summary>
    public void CreateDeck()
    {
      foreach (Suit type in this.suit)
      {
        foreach (string Value in this.cardValue)
        {
          CardVM tempcard = new CardVM(Value, type);
          tempcard.SourceRect();
          this.listOfCards.Add(tempcard);
        }
      }
      this.OnNotify();
    }

    /// <summary>
    /// Gets a random card from the deck.
    /// </summary>
    /// <returns> Returns the random card. </returns>
    public CardVM AddCard()
    {
      Random random = new Random();
      int randomNum = random.Next(1, 52);
      bool inList = true;

      if (this.usedCards.Count > 52)
      {
        return null;
      }

      if (this.usedCards.Count > 0)
      {
        while (inList)
        {
          inList = this.usedCards.Contains(randomNum);

          if (inList)
          {
            randomNum = random.Next(1, 52);
          }
          else
          {
            this.usedCards.Add(randomNum);
            return this.listOfCards[randomNum];
          }
        }
      }
      else
      {
        this.usedCards.Add(randomNum);
        return this.listOfCards[randomNum];
      }

      return null;
    }

    /// <summary>
    /// Refreshes view if property changed is not null
    /// </summary>
    /// <param name="PropertyName"> Name of the property that changed</param>
    private void OnNotify([CallerMemberName]string PropertyName = null)
    {
      if (this.PropertyChanged != null)
      {
        this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
      }
    }



    public event PropertyChangedEventHandler PropertyChanged;
  }
}
