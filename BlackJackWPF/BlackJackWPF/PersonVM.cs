using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace BlackJackWPF
{
  public class PersonVM : INotifyPropertyChanged
  {
    /// <summary>
    /// Collection of cards in the player's hand
    /// </summary>
    private ObservableCollection<CardVM> cardsInHand;

    /// <summary>
    /// Holds the player's number. Used to indicate which players is which
    /// before they are given a name by the user.
    /// </summary>
    private int playerNumber;

    /// <summary>
    /// Variable that holds the the player's current score. If they have
    /// an Ace in their hand this variable will represent their score if that 
    /// Ace is being valued as 1.
    /// </summary>
    public int Score1;

    /// <summary>
    /// Variable that holds the players current score if they have an Ace in their
    /// hand and that Ace is being valued as 11.
    /// </summary>
    public int Score2;

    /// <summary>
    /// Player's name.
    /// </summary>
    private string playerName;

    /// <summary>
    /// Stores whether or not the player busts or not.
    /// </summary>
    private bool bust;

    /// <summary>
    /// States whether or not the players hand should be visible.
    /// </summary>
    private bool handIsVisible;

    /// <summary>
    /// The money the player has
    /// </summary>
    private double money;

    /// <summary>
    /// Signifies whether the player is out of money or not
    /// </summary>
    private bool outOfMoney;

    /// <summary>
    /// The number the user enters in as their bet
    /// </summary>
    private double betText;

    /// <summary>
    /// The players bet
    /// </summary>
    private double bet;

    /// <summary>
    /// Signifies which players turn it is.
    /// </summary>
    private bool turn;

    /// <summary>
    /// Visibility of the No Money Text Block
    /// </summary>
    private Visibility noMoneyTextBlock;

    /// <summary>
    /// Visibility of the Hit and Stand Buttons
    /// </summary>
    private Visibility hitAndStandButtons;

    /// <summary>
    /// Visibility of the Bust Text Box
    /// </summary>
    private Visibility bustTextBlock;

    /// <summary>
    /// Visibility of the Bet TextBlock and TextBox
    /// </summary>
    private Visibility betVisibility;

    /// <summary>
    /// Visibility of the BetOnTable TextBlock
    /// </summary>
    private Visibility betOnTable;

    /// <summary>
    /// Constructor for PersonVM
    /// </summary>
    public PersonVM()
    {
      this.cardsInHand = new ObservableCollection<CardVM>();
      this.bust = false;
      this.handIsVisible = false;
      this.money = 500;
      this.betText = 0;
      this.outOfMoney = false;

      this.hitAndStandButtons = Visibility.Hidden;
      this.bustTextBlock = Visibility.Hidden;
      this.betVisibility = Visibility.Hidden;
      this.betOnTable = Visibility.Hidden;
      this.noMoneyTextBlock = Visibility.Hidden;
    }

    /// <summary>
    /// Gets or sets the bool associated with whether or not the player has an money
    /// </summary>
    public bool OutOfMoney
    {
      get
      {
        return this.outOfMoney;
      }
      set
      {
        this.outOfMoney = value;
        this.OnNotify();
      }
    }

    /// <summary>
    /// Gets or sets the players bet
    /// </summary>
    public double Bet
    {
      get
      {
        return this.bet;
      }
      set
      {
        this.bet = value;
        this.OnNotify();
      }
    }

    /// <summary>
    /// Gets or sets the amount the player bet
    /// </summary>
    public double BetText
    {
      get
      {
        return this.betText;
      }
      set
      {
        this.betText = value;
        this.OnNotify();
      }
    }
    
    /// <summary>
    /// Gets or sets the players money variable
    /// </summary>
    public double Money
    {
      get
      {
        return this.money;
      }
      set
      {
        this.money = value;
        this.OnNotify();
      }
    }

    /// <summary>
    /// Gets or sets the players bust variable
    /// </summary>
    public bool Bust
    {
      get
      {
        return this.bust;
      }
      set
      {
        this.bust = value;
        this.OnNotify();
      }
    }

    /// <summary>
    /// Gets or sets whether or not the players hand is visible
    /// </summary>
    public bool HandIsVisible
    {
      get
      {
        return this.handIsVisible;
      }
      set
      {
        this.handIsVisible = value;
        this.OnNotify();
      }
    }

    /// <summary>
    /// Gets or sets the players name
    /// </summary>
    public int PlayerNumber
    {
      get
      {
        return this.playerNumber;
      }
      set
      {
        this.playerNumber = value;
        this.OnNotify();
      }
    }

    /// <summary>
    /// Get's or sets the players name
    /// </summary>
    public string PlayerName
    {
      get
      {
        return this.playerName;
      }
      set
      {
        this.playerName = value;
        this.OnNotify();
      }
    }

    /// <summary>
    /// Gets or sets the player's cards
    /// </summary>
    public ObservableCollection<CardVM> CardsInHand
    {
      get
      {
        return this.cardsInHand;
      }
      private set
      {
        this.cardsInHand = value;
        this.OnNotify();
      }
    }

    /// <summary>
    /// Adds a card obtained from the deck to the players hand.
    /// </summary>
    /// <param name="deck"> The deck from which the card will be dealt</param>
    public void DealCard(DeckVM deck)
    {
      CardVM card = deck.AddCard();
      card.CardOffset = new Thickness(((this.cardsInHand.Count - 1) * 5), (this.cardsInHand.Count - 1) * 7, (this.cardsInHand.Count - 1) * -45, 0);
      // Storyboard animation = new Storyboard();
      card.CardTransform = new RotateTransform(this.cardsInHand.Count * 10, 0, 0);
      
      if (this.cardsInHand.Count < 1)
      {
        card.FaceUp = true;
      }
      else if (this.handIsVisible == true)
      {
        card.FaceUp = true;
      }
      else
      {
        card.FaceUp = false;
      }

      this.cardsInHand.Insert(0,card);

      if (this.cardsInHand.Count > 3)
      {
        for (int i = 0; i < this.cardsInHand.Count; i++)
        {
          cardsInHand[this.cardsInHand.Count - 1 - i].CardTransform = new RotateTransform((i - (this.cardsInHand.Count - 3)) * 10, 0, 0);
        }
      }

      if (this.CardsInHand.Count == 4)
      {
        for (int i = (this.cardsInHand.Count - 1); i > (this.cardsInHand.Count - 3); i--)
        {
          cardsInHand[i].CardOffset = new Thickness(
            ((((this.cardsInHand.Count - i) - 3) * -1) * -18),
            ((((this.cardsInHand.Count - i) - 1.5) * -1) * 23) * (this.cardsInHand.Count - i),
            (((this.cardsInHand.Count - i) - 1.5) * -18) * (this.cardsInHand.Count - i),
            (((this.cardsInHand.Count - i) - 1.25) * -18) * (this.cardsInHand.Count - i));
        }
      }
      else if (this.cardsInHand.Count == 5)
      {
        for (int i = (this.cardsInHand.Count - 1); i > (this.cardsInHand.Count - 4); i--)
        {
          cardsInHand[i].CardOffset = new Thickness(
            (((this.cardsInHand.Count - i) - 4) * -1) * -20,
            (((this.cardsInHand.Count - i) - 4) * -1) * 7,
            ((this.cardsInHand.Count - i) - 1) * -33,
            ((this.cardsInHand.Count - i) - 1) * 0);
        }
      }
      else if (this.cardsInHand.Count == 6)
      {
        for (int i = (this.cardsInHand.Count - 1); i > (this.cardsInHand.Count - 5); i--)
        {
          cardsInHand[i].CardOffset = new Thickness(
            (Math.Abs((this.cardsInHand.Count - i) - 5)) * -20,
            (Math.Abs((this.cardsInHand.Count - i) - 5)) * 8,
            ((this.cardsInHand.Count - i) - 1) * -35,
            ((this.cardsInHand.Count - i) - 1) * 0);
        }
      }

      this.CurrentScore();
    }

    /// <summary>
    /// Computes the player's current score based on the 
    /// cards they have in their hand.
    /// </summary>
    /// <returns> This method returns the players current score. </returns>
    public void CurrentScore()
    {
      this.Score1 = 0;
      this.Score2 = 0;
      foreach (CardVM card in this.cardsInHand)
      {
        if (card.Get_cardValue() == "Jack" || card.Get_cardValue() == "Queen")
        {
          this.Score1 += 10;
          this.Score2 += 10;
        }
        else if (card.Get_cardValue() == "King")
        {
          this.Score1 += 10;
          this.Score2 += 10;
        }
        else if (card.Get_cardValue() == "Ace")
        {
          this.Score2 += 11;
          this.Score1 += 1;
        }
        else
        {
          this.Score1 += int.Parse(card.Get_cardValue());
          this.Score2 += int.Parse(card.Get_cardValue());
        }
      }

      if (this.Score1 > 21)
      {
        this.bust = true;
      }
    }

    /// <summary>
    /// The visibility of the no money Textblock
    /// </summary>
    public Visibility NoMoney
    {
      get
      {
        return this.noMoneyTextBlock;
      }
      set
      {
        this.noMoneyTextBlock = value;
        this.OnNotify();
      }
    }
    
    /// <summary>
    /// The visibility of the Bet TextBlock and TextBox
    /// </summary>
    public Visibility BetVisibility
    {
      get
      {
        return this.betVisibility;
      }
      set
      {
        this.betVisibility = value;
        this.OnNotify();
      }
    }

    /// <summary>
    /// The visibility of the Hit and Stand buttons
    /// </summary>
    public Visibility HitAndStand
    {
      get
      {
        return this.hitAndStandButtons;
      }
      set
      {
        this.hitAndStandButtons = value;
        this.OnNotify();
      }
    }

    /// <summary>
    /// The visibility of the Bust Text Box
    /// </summary>
    public Visibility BustTextBlock
    {
      get
      {
        return this.bustTextBlock;
      }
      set
      {
        this.bustTextBlock = value;
        this.OnNotify();
      }
    }

    /// <summary>
    /// The Visibility of the BetOnTable TextBlock
    /// </summary>
    public Visibility BetOnTable
    {
      get
      {
        return this.betOnTable;
      }
      set
      {
        this.betOnTable = value;
        this.OnNotify();
      }
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
