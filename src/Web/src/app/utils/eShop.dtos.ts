/* tslint:disable */
/* Options:
Date: 2018-04-22 04:33:08
Version: 5.02
Tip: To override a DTO option, remove "//" prefix before updating
BaseUrl: http://10.0.0.201:8080

//GlobalNamespace: DTOs
//MakePropertiesOptional: True
//AddServiceStackTypes: True
//AddResponseStatus: False
//AddImplicitVersion:
//AddDescriptionAsComments: True
//IncludeTypes:
//ExcludeTypes:
//DefaultImports:
*/


export module DTOs
{

    export interface IReturn<T>
    {
        createResponse() : T;
    }

    export interface IReturnVoid
    {
        createResponse() : void;
    }

    export class Query<T>
    {
    }

    export class Basket
    {
        id: string;
        customerId: string;
        customer: string;
        totalQuantity: number;
        subTotal: number;
        total: number;
        created: string;
        updated: string;
    }

    // @DataContract
    export class ResponseError
    {
        // @DataMember(Order=1, EmitDefaultValue=false)
        errorCode: string;

        // @DataMember(Order=2, EmitDefaultValue=false)
        fieldName: string;

        // @DataMember(Order=3, EmitDefaultValue=false)
        message: string;

        // @DataMember(Order=4, EmitDefaultValue=false)
        meta: { [index:string]: string; };
    }

    // @DataContract
    export class ResponseStatus
    {
        // @DataMember(Order=1)
        errorCode: string;

        // @DataMember(Order=2)
        message: string;

        // @DataMember(Order=3)
        stackTrace: string;

        // @DataMember(Order=4)
        errors: ResponseError[];

        // @DataMember(Order=5)
        meta: { [index:string]: string; };
    }

    export class CommandResponse
    {
        roundTripMs: number;
        responseStatus: ResponseStatus;
    }

    export class DomainCommand
    {
    }

    export class Paged<T>
    {
    }

    export class Items
    {
        basketId: string;
        itemId: string;
        productId: string;
        productName: string;
        productPrice: number;
        quantity: number;
        total: number;
    }

    export class CategoryBrand
    {
        id: string;
        brand: string;
    }

    export class CategoryType
    {
        id: string;
        type: string;
    }

    export class Product
    {
        id: string;
        name: string;
        description: string;
        price: number;
        catalogTypeId: string;
        catalogType: string;
        catalogBrandId: string;
        catalogBrand: string;
        availableStock: number;
        restockThreshold: number;
        maxStockThrshold: number;
        onReorder: number;
        pictureContents: Uint8Array;
        pictureContentType: string;
    }

    export class ProductIndex
    {
        id: string;
        name: string;
        description: string;
        price: number;
        catalogTypeId: string;
        catalogType: string;
        catalogBrandId: string;
        catalogBrand: string;
        availableStock: number;
        restockThreshold: number;
        maxStockThrshold: number;
        onReorder: number;
        pictureContents: Uint8Array;
        pictureContentType: string;
    }

    export class StampedCommand
    {
        stamp: number;
    }

    export class Location extends StampedCommand
    {
        id: string;
        code: string;
        description: string;
        points: Point[];
    }

    export class Campaign
    {
        id: string;
        name: string;
        description: string;
        start: string;
        end: string;
        pictureContents: Uint8Array;
        pictureContentType: string;
    }

    export class Buyer
    {
        id: string;
        givenName: string;
    }

    export class Address
    {
        id: string;
        buyerId: string;
        street: string;
        city: string;
        state: string;
        country: string;
        zipCode: string;
    }

    export class PaymentMethod
    {
        id: string;
        buyerId: string;
        alias: string;
        cardNumber: string;
        securityNumber: string;
        cardholderName: string;
        expiration: string;
        cardType: string;
    }

    export class Order
    {
        id: string;
        status: string;
        statusDescription: string;
        buyerId: string;
        buyerName: string;
        addressId: string;
        address: string;
        cityState: string;
        zipCode: string;
        country: string;
        paymentMethodId: string;
        paymentMethod: string;
        quantity: number;
        subTotal: number;
        total: number;
    }

    export class OrderIndex
    {
        id: string;
        status: string;
        statusDescription: string;
        buyerId: string;
        buyerName: string;
        addressId: string;
        address: string;
        cityState: string;
        zipCode: string;
        country: string;
        paymentMethodId: string;
        paymentMethod: string;
        quantity: number;
        subTotal: number;
        total: number;
    }

    export class Item
    {
        id: string;
        orderId: string;
        productId: string;
        productName: string;
        productPrice: number;
        quantity: number;
    }

    export class Point
    {
        id: string;
        locationId: string;
        longitude: number;
        latitude: number;
    }

    export interface ICommand
    {
    }

    export interface IMessage
    {
    }

    export class QueryResponse<T>
    {
        roundTripMs: number;
        payload: T;
    }

    export class PagedResponse<T>
    {
        roundTripMs: number;
        total: number;
        records: T[];
    }

    /**
    * Basket
    */
    // @Route("/basket", "GET")
    // @Api(Description="Basket")
    export class GetBasket extends Query<Basket> implements IReturn<QueryResponse<Basket>>
    {
        basketId: string;
        createResponse() { return new QueryResponse<Basket>(); }
        getTypeName() { return "GetBasket"; }
    }

    /**
    * Basket
    */
    // @Route("/basket", "DELETE")
    // @Api(Description="Basket")
    export class BasketDestroy extends DomainCommand implements IReturn<CommandResponse>
    {
        basketId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "BasketDestroy"; }
    }

    /**
    * Basket
    */
    // @Route("/basket/item", "GET")
    // @Api(Description="Basket")
    export class GetBasketItems extends Paged<Items> implements IReturn<PagedResponse<Items>>
    {
        basketId: string;
        createResponse() { return new PagedResponse<Items>(); }
        getTypeName() { return "GetBasketItems"; }
    }

    /**
    * Basket
    */
    // @Route("/basket/item", "POST")
    // @Api(Description="Basket")
    export class AddBasketItem extends DomainCommand implements IReturn<CommandResponse>
    {
        basketId: string;
        itemId: string;
        productId: string;
        quantity: number;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "AddBasketItem"; }
    }

    /**
    * Basket
    */
    // @Route("/basket/item/{ItemId}", "DELETE")
    // @Api(Description="Basket")
    export class RemoveBasketItem extends DomainCommand implements IReturn<CommandResponse>
    {
        basketId: string;
        itemId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "RemoveBasketItem"; }
    }

    /**
    * Basket
    */
    // @Route("/basket/item/{ItemId}/quantity", "POST")
    // @Api(Description="Basket")
    export class UpdateBasketItemQuantity extends DomainCommand implements IReturn<CommandResponse>
    {
        basketId: string;
        itemId: string;
        quantity: number;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "UpdateBasketItemQuantity"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/brand", "GET")
    // @Api(Description="Catalog")
    export class ListCategoryBrands extends Paged<CategoryBrand> implements IReturn<PagedResponse<CategoryBrand>>
    {
        term: string;
        limit: number;
        createResponse() { return new PagedResponse<CategoryBrand>(); }
        getTypeName() { return "ListCategoryBrands"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/brand", "POST")
    // @Api(Description="Catalog")
    export class AddCategoryBrand extends DomainCommand implements IReturn<CommandResponse>
    {
        brandId: string;
        brand: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "AddCategoryBrand"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/brand/{BrandId}", "DELETE")
    // @Api(Description="Catalog")
    export class RemoveCategoryBrand extends DomainCommand implements IReturn<CommandResponse>
    {
        brandId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "RemoveCategoryBrand"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/type", "GET")
    // @Api(Description="Catalog")
    export class ListCategoryTypes extends Paged<CategoryType> implements IReturn<PagedResponse<CategoryType>>
    {
        term: string;
        limit: number;
        createResponse() { return new PagedResponse<CategoryType>(); }
        getTypeName() { return "ListCategoryTypes"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/type", "POST")
    // @Api(Description="Catalog")
    export class AddCategoryType extends DomainCommand implements IReturn<CommandResponse>
    {
        typeId: string;
        type: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "AddCategoryType"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/type/{TypeId}", "POST")
    // @Api(Description="Catalog")
    export class RemoveCategoryType extends DomainCommand implements IReturn<CommandResponse>
    {
        typeId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "RemoveCategoryType"; }
    }

    export class GetProduct extends Query<Product> implements IReturn<QueryResponse<Product>>
    {
        productId: string;
        createResponse() { return new QueryResponse<Product>(); }
        getTypeName() { return "GetProduct"; }
    }

    export class ListProducts extends Paged<ProductIndex> implements IReturn<PagedResponse<ProductIndex>>
    {
        createResponse() { return new PagedResponse<ProductIndex>(); }
        getTypeName() { return "ListProducts"; }
    }

    export class AddProduct extends DomainCommand implements IReturn<CommandResponse>
    {
        productId: string;
        name: string;
        price: number;
        categoryBrandId: string;
        categoryTypeId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "AddProduct"; }
    }

    export class RemoveProduct extends DomainCommand implements IReturn<CommandResponse>
    {
        productId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "RemoveProduct"; }
    }

    export class SetPictureProduct extends DomainCommand implements IReturn<CommandResponse>
    {
        productId: string;
        pictureUrl: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "SetPictureProduct"; }
    }

    export class UpdateDescriptionProduct extends DomainCommand implements IReturn<CommandResponse>
    {
        productId: string;
        description: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "UpdateDescriptionProduct"; }
    }

    export class UpdatePriceProduct extends DomainCommand implements IReturn<CommandResponse>
    {
        productId: string;
        price: number;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "UpdatePriceProduct"; }
    }

    /**
    * Identity
    */
    // @Route("/identity/register", "POST")
    // @Api(Description="Identity")
    export class UserRegister extends DomainCommand implements IReturn<CommandResponse>
    {
        userId: string;
        givenName: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "UserRegister"; }
    }

    /**
    * Location
    */
    // @Route("/location", "GET")
    // @Api(Description="Location")
    export class ListLocations extends Paged<Location> implements IReturn<PagedResponse<Location>>
    {
        createResponse() { return new PagedResponse<Location>(); }
        getTypeName() { return "ListLocations"; }
    }

    /**
    * Location
    */
    // @Route("/location", "POST")
    // @Api(Description="Location")
    export class AddLocation extends DomainCommand implements IReturn<CommandResponse>
    {
        locationId: string;
        code: string;
        description: string;
        parentId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "AddLocation"; }
    }

    /**
    * Location
    */
    // @Route("/location/{LocationId}", "DELETE")
    // @Api(Description="Location")
    export class RemoveLocation extends DomainCommand implements IReturn<CommandResponse>
    {
        locationId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "RemoveLocation"; }
    }

    /**
    * Location
    */
    // @Route("/location/{LocationId}/description", "POST")
    // @Api(Description="Location")
    export class UpdateDescriptionLocation extends DomainCommand implements IReturn<CommandResponse>
    {
        locationId: string;
        description: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "UpdateDescriptionLocation"; }
    }

    /**
    * Location
    */
    // @Route("/location/{LocationId}/point", "POST")
    // @Api(Description="Location")
    export class AddPoint extends DomainCommand implements IReturn<CommandResponse>
    {
        locationId: string;
        pointId: string;
        longitude: number;
        latitude: number;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "AddPoint"; }
    }

    /**
    * Location
    */
    // @Route("/location/{LocationId}/point/{PointId}", "DELETE")
    // @Api(Description="Location")
    export class RemovePoint extends DomainCommand implements IReturn<CommandResponse>
    {
        locationId: string;
        pointId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "RemovePoint"; }
    }

    export class GetCampaign extends Query<Campaign> implements IReturn<QueryResponse<Campaign>>
    {
        campaignId: string;
        createResponse() { return new QueryResponse<Campaign>(); }
        getTypeName() { return "GetCampaign"; }
    }

    export class ListCampaigns extends Paged<Campaign> implements IReturn<PagedResponse<Campaign>>
    {
        createResponse() { return new PagedResponse<Campaign>(); }
        getTypeName() { return "ListCampaigns"; }
    }

    /**
    * Marketing
    */
    // @Route("/campaign/{CampaignId}/description", "POST")
    // @Api(Description="Marketing")
    export class ChangeDescriptionCampaign extends DomainCommand implements IReturn<CommandResponse>
    {
        campaignId: string;
        description: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "ChangeDescriptionCampaign"; }
    }

    /**
    * Marketing
    */
    // @Route("/campaign", "POST")
    // @Api(Description="Marketing")
    export class DefineCampaign extends DomainCommand implements IReturn<CommandResponse>
    {
        campaignId: string;
        name: string;
        description: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "DefineCampaign"; }
    }

    /**
    * Marketing
    */
    // @Route("/campaign/{CampaignId}/period", "POST")
    // @Api(Description="Marketing")
    export class SetPeriodCampaign extends DomainCommand implements IReturn<CommandResponse>
    {
        campaignId: string;
        start: string;
        end: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "SetPeriodCampaign"; }
    }

    /**
    * Marketing
    */
    // @Route("/campaign/{CampaignId}/picture", "POST")
    // @Api(Description="Marketing")
    export class SetPictureCampaign extends DomainCommand implements IReturn<CommandResponse>
    {
        campaignId: string;
        pictureUrl: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "SetPictureCampaign"; }
    }

    /**
    * Marketing
    */
    // @Route("/campaign/{CampaignId}/rule", "POST")
    // @Api(Description="Marketing")
    export class AddCampaignRule extends DomainCommand implements IReturn<CommandResponse>
    {
        campaignId: string;
        ruleId: string;
        description: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "AddCampaignRule"; }
    }

    /**
    * Marketing
    */
    // @Route("/campaign/{CampaignId}/rule/{RuleId}", "DELETE")
    // @Api(Description="Marketing")
    export class RemoveCampaignRule extends DomainCommand implements IReturn<CommandResponse>
    {
        campaignId: string;
        ruleId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "RemoveCampaignRule"; }
    }

    /**
    * Ordering
    */
    // @Route("/buyer", "GET")
    // @Api(Description="Ordering")
    export class Buyers extends Paged<Buyer> implements IReturn<PagedResponse<Buyer>>
    {
        createResponse() { return new PagedResponse<Buyer>(); }
        getTypeName() { return "Buyers"; }
    }

    /**
    * Ordering
    */
    // @Route("/buyer", "POST")
    // @Api(Description="Ordering")
    export class CreateBuyer extends DomainCommand implements IReturn<CommandResponse>
    {
        buyerId: string;
        givenName: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "CreateBuyer"; }
    }

    /**
    * Ordering
    */
    // @Route("/buyer/{BuyerId}/address", "GET")
    // @Api(Description="Ordering")
    export class ListAddresses extends Paged<Address> implements IReturn<PagedResponse<Address>>
    {
        buyerId: string;
        createResponse() { return new PagedResponse<Address>(); }
        getTypeName() { return "ListAddresses"; }
    }

    /**
    * Ordering
    */
    // @Route("/buyer/{BuyerId}/address", "POST")
    // @Api(Description="Ordering")
    export class AddBuyerAddress extends DomainCommand implements IReturn<CommandResponse>
    {
        buyerId: string;
        addressId: string;
        street: string;
        city: string;
        state: string;
        country: string;
        zipCode: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "AddBuyerAddress"; }
    }

    /**
    * Ordering
    */
    // @Route("/buyer/{BuyerId}/address/{AddressId}", "DELETE")
    // @Api(Description="Ordering")
    export class RemoveBuyerAddress extends DomainCommand implements IReturn<CommandResponse>
    {
        buyerId: string;
        addressId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "RemoveBuyerAddress"; }
    }

    /**
    * Ordering
    */
    // @Route("/buyer/{BuyerId}/payment_method", "GET")
    // @Api(Description="Ordering")
    export class ListPaymentMethods extends Paged<PaymentMethod> implements IReturn<PagedResponse<PaymentMethod>>
    {
        buyerId: string;
        createResponse() { return new PagedResponse<PaymentMethod>(); }
        getTypeName() { return "ListPaymentMethods"; }
    }

    /**
    * Ordering
    */
    // @Route("/buyer/{BuyerId}/payment_method", "POST")
    // @Api(Description="Ordering")
    export class AddBuyerPaymentMethod extends DomainCommand implements IReturn<CommandResponse>
    {
        buyerId: string;
        paymentMethodId: string;
        alias: string;
        cardNumber: string;
        securityNumber: string;
        cardholderName: string;
        expiration: string;
        cardType: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "AddBuyerPaymentMethod"; }
    }

    /**
    * Ordering
    */
    // @Route("/buyer/{BuyerId}/payment_method/{PaymentMethodId}", "DELETE")
    // @Api(Description="Ordering")
    export class RemoveBuyerPaymentMethod extends DomainCommand implements IReturn<CommandResponse>
    {
        buyerId: string;
        paymentMethodId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "RemoveBuyerPaymentMethod"; }
    }

    /**
    * Ordering
    */
    // @Route("/order/{OrderId}", "GET")
    // @Api(Description="Ordering")
    export class GetOrder extends Query<Order> implements IReturn<QueryResponse<Order>>
    {
        orderId: string;
        createResponse() { return new QueryResponse<Order>(); }
        getTypeName() { return "GetOrder"; }
    }

    /**
    * Ordering
    */
    // @Route("/order", "GET")
    // @Api(Description="Ordering")
    export class ListOrders extends Paged<OrderIndex> implements IReturn<PagedResponse<OrderIndex>>
    {
        createResponse() { return new PagedResponse<OrderIndex>(); }
        getTypeName() { return "ListOrders"; }
    }

    /**
    * Ordering
    */
    // @Route("/order/{OrderId}/cancel", "POST")
    // @Api(Description="Ordering")
    export class CancelOrder extends DomainCommand implements IReturn<CommandResponse>
    {
        orderId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "CancelOrder"; }
    }

    /**
    * Ordering
    */
    // @Route("/order/{OrderId}/confirm", "POST")
    // @Api(Description="Ordering")
    export class ConfirmOrder extends DomainCommand implements IReturn<CommandResponse>
    {
        orderId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "ConfirmOrder"; }
    }

    /**
    * Ordering
    */
    // @Route("/order", "POST")
    // @Api(Description="Ordering")
    export class DraftOrder extends DomainCommand implements IReturn<CommandResponse>
    {
        orderId: string;
        buyerId: string;
        basketId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "DraftOrder"; }
    }

    /**
    * Ordering
    */
    // @Route("/order/{OrderId}/pay", "POST")
    // @Api(Description="Ordering")
    export class PayOrder extends DomainCommand implements IReturn<CommandResponse>
    {
        orderId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "PayOrder"; }
    }

    /**
    * Ordering
    */
    // @Route("/order/{OrderId}/ship", "POST")
    // @Api(Description="Ordering")
    export class ShipOrder extends DomainCommand implements IReturn<CommandResponse>
    {
        orderId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "ShipOrder"; }
    }

    /**
    * Ordering
    */
    // @Route("/order/{OrderId}/address", "POST")
    // @Api(Description="Ordering")
    export class SetAddressOrder extends DomainCommand implements IReturn<CommandResponse>
    {
        orderId: string;
        addressId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "SetAddressOrder"; }
    }

    /**
    * Ordering
    */
    // @Route("/order/{OrderId}/payment_method", "POST")
    // @Api(Description="Ordering")
    export class SetPaymentMethodOrder extends DomainCommand implements IReturn<CommandResponse>
    {
        orderId: string;
        paymentMethodId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "SetPaymentMethodOrder"; }
    }

    /**
    * Ordering
    */
    // @Route("/order/{OrderId}/item", "GET")
    // @Api(Description="Ordering")
    export class ListOrderItems extends Paged<Item> implements IReturn<PagedResponse<Item>>
    {
        orderId: string;
        createResponse() { return new PagedResponse<Item>(); }
        getTypeName() { return "ListOrderItems"; }
    }

    /**
    * Ordering
    */
    // @Route("/order/{OrderId}/item", "POST")
    // @Api(Description="Ordering")
    export class AddOrderItem extends DomainCommand implements IReturn<CommandResponse>
    {
        itemId: string;
        orderId: string;
        productId: string;
        quantity: number;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "AddOrderItem"; }
    }

    /**
    * Ordering
    */
    // @Route("/order/{OrderId}/item/{ItemId}/quantity", "POST")
    // @Api(Description="Ordering")
    export class ChangeQuantityOrderItem extends DomainCommand implements IReturn<CommandResponse>
    {
        itemId: string;
        orderId: string;
        quantity: number;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "ChangeQuantityOrderItem"; }
    }

    /**
    * Ordering
    */
    // @Route("/order/{OrderId}/item/{ItemId}", "DELETE")
    // @Api(Description="Ordering")
    export class RemoveOrderItem extends DomainCommand implements IReturn<CommandResponse>
    {
        itemId: string;
        orderId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "RemoveOrderItem"; }
    }

}
