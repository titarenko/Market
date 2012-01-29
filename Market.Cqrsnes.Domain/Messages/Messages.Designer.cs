using System;
using Cqrsnes.Infrastructure;

namespace Market.Cqrsnes.Domain.Messages
{
	public class CreateUser : Command
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Password { get; set; }
	}

	public class UserCreated : Event
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string PasswordHash { get; set; }
		public string PasswordSalt { get; set; }
	}

	public class LogIn : Command
	{
		public Guid Id { get; set; }
		public string Password { get; set; }
	}

	public class LoggedIn : Event
	{
		public Guid Id { get; set; }
	}

	public class LogInFailed : Event
	{
		public Guid Id { get; set; }
	}

	public class LogOut : Command
	{
		public Guid Id { get; set; }
	}

	public class LoggedOut : Event
	{
		public Guid Id { get; set; }
	}

	public class LogOutFailed : Event
	{
		public Guid Id { get; set; }
	}

	public class CreateStore : Command
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public Guid OwnerId { get; set; }
	}

	public class StoreCreated : Event
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public Guid OwnerId { get; set; }
	}

	public class StoreCreationFailed : Event
	{
		public Guid Id { get; set; }
	}

	public class StoreDuplicateFound : Event
	{
		public string Name { get; set; }
		public Guid OwnerId { get; set; }
	}

	public class CreateArticle : Command
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
	}

	public class ArticleCreated : Event
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
	}

	public class ArticleCreationFailed : Event
	{
		public Guid Id { get; set; }
	}

	public class ArticleDuplicateFound : Event
	{
		public string Name { get; set; }
	}

	public class CreateOffer : Command
	{
		public Guid Id { get; set; }
		public Guid StoreId { get; set; }
		public Guid ArticleId { get; set; }
		public int Count { get; set; }
		public double Price { get; set; }
	}

	public class OfferCreated : Event
	{
		public Guid Id { get; set; }
		public Guid StoreId { get; set; }
		public Guid ArticleId { get; set; }
		public int Count { get; set; }
		public double Price { get; set; }
	}

	public class CreatePurchase : Command
	{
		public Guid Id { get; set; }
		public Guid OfferId { get; set; }
		public Guid BuyerId { get; set; }
		public int Count { get; set; }
	}

	public class PurchaseCreated : Event
	{
		public Guid Id { get; set; }
		public Guid OfferId { get; set; }
		public Guid BuyerId { get; set; }
		public int Count { get; set; }
	}

	public class ReserveMoney : Command
	{
		public Guid PurchaseId { get; set; }
	}

	public class MoneyReserved : Event
	{
		public Guid PurchaseId { get; set; }
	}

	public class MoneyReservationFailed : Event
	{
		public Guid PurchaseId { get; set; }
	}

	public class ReserveArticle : Command
	{
		public Guid PurchaseId { get; set; }
	}

	public class ArticleReserved : Event
	{
		public Guid PurchaseId { get; set; }
	}

	public class ArticleReservationFailed : Event
	{
		public Guid PurchaseId { get; set; }
	}

	public class DecreaseBalance : Command
	{
		public Guid PurchaseId { get; set; }
	}

	public class BalanceDecreased : Event
	{
		public Guid PurchaseId { get; set; }
	}

	public class CancelMoneyReservaion : Command
	{
		public Guid PurchaseId { get; set; }
	}

	public class MoneyReservaionCanceled : Event
	{
		public Guid PurchaseId { get; set; }
	}

	public class DecreaseArticleCount : Command
	{
		public Guid PurchaseId { get; set; }
	}

	public class ArticleCountDecreased : Event
	{
		public Guid PurchaseId { get; set; }
	}

	public class CancelArticleReservation : Command
	{
		public Guid PurchaseId { get; set; }
	}

	public class ArticleReservationCanceled : Event
	{
		public Guid PurchaseId { get; set; }
	}

	public class PurchaseSucceeded : Event
	{
		public Guid Id { get; set; }
	}

	public class PurchaseFailed : Event
	{
		public Guid Id { get; set; }
	}
}
