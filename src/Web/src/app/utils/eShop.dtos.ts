/* tslint:disable */
/* Options:
Date: 2018-05-16 05:48:11
Version: 5.10
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

    export interface IPost
    {
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
        totalFees: number;
        totalTaxes: number;
        total: number;
        created: number;
        updated: number;
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

    export class Item
    {
        id: string;
        basketId: string;
        productId: string;
        productPictureContents: Uint8Array;
        productPictureContentType: string;
        productName: string;
        productDescription: string;
        productPrice: number;
        quantity: number;
        additionalTaxes: number;
        additionalFees: number;
        total: number;
    }

    export class CatalogBrand
    {
        id: string;
        brand: string;
    }

    export class CatalogType
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
        maxStockThreshold: number;
        onReorder: boolean;
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
        maxStockThreshold: number;
        onReorder: boolean;
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

    export class Status
    {
        id: string;
        isSetup: boolean;
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

    // @DataContract
    export class AuthenticateResponse
    {
        // @DataMember(Order=1)
        userId: string;

        // @DataMember(Order=2)
        sessionId: string;

        // @DataMember(Order=3)
        userName: string;

        // @DataMember(Order=4)
        displayName: string;

        // @DataMember(Order=5)
        referrerUrl: string;

        // @DataMember(Order=6)
        bearerToken: string;

        // @DataMember(Order=7)
        refreshToken: string;

        // @DataMember(Order=8)
        responseStatus: ResponseStatus;

        // @DataMember(Order=9)
        meta: { [index:string]: string; };
    }

    // @DataContract
    export class AssignRolesResponse
    {
        // @DataMember(Order=1)
        allRoles: string[];

        // @DataMember(Order=2)
        allPermissions: string[];

        // @DataMember(Order=3)
        responseStatus: ResponseStatus;
    }

    // @DataContract
    export class UnAssignRolesResponse
    {
        // @DataMember(Order=1)
        allRoles: string[];

        // @DataMember(Order=2)
        allPermissions: string[];

        // @DataMember(Order=3)
        responseStatus: ResponseStatus;
    }

    // @DataContract
    export class ConvertSessionToTokenResponse
    {
        // @DataMember(Order=1)
        meta: { [index:string]: string; };

        // @DataMember(Order=2)
        accessToken: string;

        // @DataMember(Order=3)
        responseStatus: ResponseStatus;
    }

    // @DataContract
    export class GetAccessTokenResponse
    {
        // @DataMember(Order=1)
        accessToken: string;

        // @DataMember(Order=2)
        responseStatus: ResponseStatus;
    }

    // @DataContract
    export class RegisterResponse
    {
        // @DataMember(Order=1)
        userId: string;

        // @DataMember(Order=2)
        sessionId: string;

        // @DataMember(Order=3)
        userName: string;

        // @DataMember(Order=4)
        referrerUrl: string;

        // @DataMember(Order=5)
        bearerToken: string;

        // @DataMember(Order=6)
        refreshToken: string;

        // @DataMember(Order=7)
        responseStatus: ResponseStatus;

        // @DataMember(Order=8)
        meta: { [index:string]: string; };
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

    // @Route("/auth")
    // @Route("/auth/{provider}")
    // @Route("/authenticate")
    // @Route("/authenticate/{provider}")
    // @DataContract
    export class Authenticate implements IReturn<AuthenticateResponse>, IPost
    {
        // @DataMember(Order=1)
        provider: string;

        // @DataMember(Order=2)
        state: string;

        // @DataMember(Order=3)
        oauth_token: string;

        // @DataMember(Order=4)
        oauth_verifier: string;

        // @DataMember(Order=5)
        userName: string;

        // @DataMember(Order=6)
        password: string;

        // @DataMember(Order=7)
        rememberMe: boolean;

        // @DataMember(Order=8)
        continue: string;

        // @DataMember(Order=9)
        nonce: string;

        // @DataMember(Order=10)
        uri: string;

        // @DataMember(Order=11)
        response: string;

        // @DataMember(Order=12)
        qop: string;

        // @DataMember(Order=13)
        nc: string;

        // @DataMember(Order=14)
        cnonce: string;

        // @DataMember(Order=15)
        useTokenCookie: boolean;

        // @DataMember(Order=16)
        accessToken: string;

        // @DataMember(Order=17)
        accessTokenSecret: string;

        // @DataMember(Order=18)
        meta: { [index:string]: string; };
        createResponse() { return new AuthenticateResponse(); }
        getTypeName() { return "Authenticate"; }
    }

    // @Route("/assignroles")
    // @DataContract
    export class AssignRoles implements IReturn<AssignRolesResponse>, IPost
    {
        // @DataMember(Order=1)
        userName: string;

        // @DataMember(Order=2)
        permissions: string[];

        // @DataMember(Order=3)
        roles: string[];
        createResponse() { return new AssignRolesResponse(); }
        getTypeName() { return "AssignRoles"; }
    }

    // @Route("/unassignroles")
    // @DataContract
    export class UnAssignRoles implements IReturn<UnAssignRolesResponse>, IPost
    {
        // @DataMember(Order=1)
        userName: string;

        // @DataMember(Order=2)
        permissions: string[];

        // @DataMember(Order=3)
        roles: string[];
        createResponse() { return new UnAssignRolesResponse(); }
        getTypeName() { return "UnAssignRoles"; }
    }

    // @Route("/session-to-token")
    // @DataContract
    export class ConvertSessionToToken implements IReturn<ConvertSessionToTokenResponse>, IPost
    {
        // @DataMember(Order=1)
        preserveSession: boolean;
        createResponse() { return new ConvertSessionToTokenResponse(); }
        getTypeName() { return "ConvertSessionToToken"; }
    }

    // @Route("/access-token")
    // @DataContract
    export class GetAccessToken implements IReturn<GetAccessTokenResponse>, IPost
    {
        // @DataMember(Order=1)
        refreshToken: string;
        createResponse() { return new GetAccessTokenResponse(); }
        getTypeName() { return "GetAccessToken"; }
    }

    // @Route("/register")
    // @DataContract
    export class Register implements IReturn<RegisterResponse>, IPost
    {
        // @DataMember(Order=1)
        userName: string;

        // @DataMember(Order=2)
        firstName: string;

        // @DataMember(Order=3)
        lastName: string;

        // @DataMember(Order=4)
        displayName: string;

        // @DataMember(Order=5)
        email: string;

        // @DataMember(Order=6)
        password: string;

        // @DataMember(Order=7)
        autoLogin: boolean;

        // @DataMember(Order=8)
        continue: string;
        createResponse() { return new RegisterResponse(); }
        getTypeName() { return "Register"; }
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
    // @Route("/basket", "POST")
    // @Api(Description="Basket")
    export class InitiateBasket extends DomainCommand implements IReturn<CommandResponse>
    {
        basketId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "InitiateBasket"; }
    }

    /**
    * Basket
    */
    // @Route("/basket/claim", "POST")
    // @Api(Description="Basket")
    export class ClaimBasket extends DomainCommand implements IReturn<CommandResponse>
    {
        basketId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "ClaimBasket"; }
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
    export class GetBasketItems extends Paged<Item> implements IReturn<PagedResponse<Item>>
    {
        basketId: string;
        createResponse() { return new PagedResponse<Item>(); }
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
        productId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "AddBasketItem"; }
    }

    /**
    * Basket
    */
    // @Route("/basket/item/{ProductId}", "DELETE")
    // @Api(Description="Basket")
    export class RemoveBasketItem extends DomainCommand implements IReturn<CommandResponse>
    {
        basketId: string;
        productId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "RemoveBasketItem"; }
    }

    /**
    * Basket
    */
    // @Route("/basket/item/{ProductId}/quantity", "POST")
    // @Api(Description="Basket")
    export class UpdateBasketItemQuantity extends DomainCommand implements IReturn<CommandResponse>
    {
        basketId: string;
        productId: string;
        quantity: number;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "UpdateBasketItemQuantity"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/brand", "GET")
    // @Api(Description="Catalog")
    export class ListCatalogBrands extends Paged<CatalogBrand> implements IReturn<PagedResponse<CatalogBrand>>
    {
        term: string;
        limit: number;
        id: string;
        createResponse() { return new PagedResponse<CatalogBrand>(); }
        getTypeName() { return "ListCatalogBrands"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/brand", "POST")
    // @Api(Description="Catalog")
    export class AddCatalogBrand extends DomainCommand implements IReturn<CommandResponse>
    {
        brandId: string;
        brand: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "AddCatalogBrand"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/brand/{BrandId}", "DELETE")
    // @Api(Description="Catalog")
    export class RemoveCatalogBrand extends DomainCommand implements IReturn<CommandResponse>
    {
        brandId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "RemoveCatalogBrand"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/type", "GET")
    // @Api(Description="Catalog")
    export class ListCatalogTypes extends Paged<CatalogType> implements IReturn<PagedResponse<CatalogType>>
    {
        term: string;
        limit: number;
        id: string;
        createResponse() { return new PagedResponse<CatalogType>(); }
        getTypeName() { return "ListCatalogTypes"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/type", "POST")
    // @Api(Description="Catalog")
    export class AddCatalogType extends DomainCommand implements IReturn<CommandResponse>
    {
        typeId: string;
        type: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "AddCatalogType"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/type/{TypeId}", "POST")
    // @Api(Description="Catalog")
    export class RemoveCatalogType extends DomainCommand implements IReturn<CommandResponse>
    {
        typeId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "RemoveCatalogType"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/products/{ProductId}", "GET")
    // @Api(Description="Catalog")
    export class GetProduct extends Query<Product> implements IReturn<QueryResponse<Product>>
    {
        productId: string;
        createResponse() { return new QueryResponse<Product>(); }
        getTypeName() { return "GetProduct"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/products", "GET")
    // @Api(Description="Catalog")
    export class ListProducts extends Paged<ProductIndex> implements IReturn<PagedResponse<ProductIndex>>
    {
        createResponse() { return new PagedResponse<ProductIndex>(); }
        getTypeName() { return "ListProducts"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog", "GET")
    // @Api(Description="Catalog")
    export class Catalog extends Paged<ProductIndex> implements IReturn<PagedResponse<ProductIndex>>
    {
        brandId: string;
        typeId: string;
        search: string;
        createResponse() { return new PagedResponse<ProductIndex>(); }
        getTypeName() { return "Catalog"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/products", "POST")
    // @Api(Description="Catalog")
    export class AddProduct extends DomainCommand implements IReturn<CommandResponse>
    {
        productId: string;
        name: string;
        price: number;
        catalogBrandId: string;
        catalogTypeId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "AddProduct"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/products/{ProductId}", "DELETE")
    // @Api(Description="Catalog")
    export class RemoveProduct extends DomainCommand implements IReturn<CommandResponse>
    {
        productId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "RemoveProduct"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/products/{ProductId}/picture", "POST")
    // @Api(Description="Catalog")
    export class SetPictureProduct extends DomainCommand implements IReturn<CommandResponse>
    {
        productId: string;
        content: string;
        contentType: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "SetPictureProduct"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/products/{ProductId}/description", "POST")
    // @Api(Description="Catalog")
    export class UpdateDescriptionProduct extends DomainCommand implements IReturn<CommandResponse>
    {
        productId: string;
        description: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "UpdateDescriptionProduct"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/products/{ProductId}/mark", "POST")
    // @Api(Description="Catalog")
    export class MarkReordered extends DomainCommand implements IReturn<CommandResponse>
    {
        productId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "MarkReordered"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/products/{ProductId}/unmark", "POST")
    // @Api(Description="Catalog")
    export class UnMarkReordered extends DomainCommand implements IReturn<CommandResponse>
    {
        productId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "UnMarkReordered"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/products/{ProductId}/stock", "POST")
    // @Api(Description="Catalog")
    export class UpdateStock extends DomainCommand implements IReturn<CommandResponse>
    {
        productId: string;
        stock: number;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "UpdateStock"; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/products/{ProductId}/price", "POST")
    // @Api(Description="Catalog")
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
    // @Route("/identity/user", "GET")
    // @Api(Description="Identity")
    export class GetIdentity extends DomainCommand implements IReturn<CommandResponse>
    {
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "GetIdentity"; }
    }

    /**
    * Identity
    */
    // @Route("/identity/users", "GET")
    // @Api(Description="Identity")
    export class GetUsers
    {
    }

    /**
    * Identity
    */
    // @Route("/identity/users", "POST")
    // @Api(Description="Identity")
    export class UserRegister extends DomainCommand implements IReturn<CommandResponse>
    {
        givenName: string;
        userName: string;
        password: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "UserRegister"; }
    }

    /**
    * Identity
    */
    // @Route("/identity/users/{UserName}/name", "POST")
    // @Api(Description="Identity")
    export class ChangeName extends DomainCommand implements IReturn<CommandResponse>
    {
        userName: string;
        givenName: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "ChangeName"; }
    }

    /**
    * Identity
    */
    // @Route("/identity/users/{UserName}/password", "POST")
    // @Api(Description="Identity")
    export class ChangePassword extends DomainCommand implements IReturn<CommandResponse>
    {
        userName: string;
        password: string;
        newPassword: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "ChangePassword"; }
    }

    /**
    * Identity
    */
    // @Route("/identity/users/{UserName}/enable", "POST")
    // @Api(Description="Identity")
    export class UserEnable extends DomainCommand implements IReturn<CommandResponse>
    {
        userName: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "UserEnable"; }
    }

    /**
    * Identity
    */
    // @Route("/identity/users/{UserName}/disable", "POST")
    // @Api(Description="Identity")
    export class UserDisable extends DomainCommand implements IReturn<CommandResponse>
    {
        userName: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "UserDisable"; }
    }

    /**
    * Identity
    */
    // @Route("/identity/users/{UserName}/assign", "POST")
    // @Api(Description="Identity")
    export class AssignRole extends DomainCommand implements IReturn<CommandResponse>
    {
        userName: string;
        roleId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "AssignRole"; }
    }

    /**
    * Identity
    */
    // @Route("/identity/users/{UserName}/revoke", "POST")
    // @Api(Description="Identity")
    export class RevokeRole extends DomainCommand implements IReturn<CommandResponse>
    {
        userName: string;
        roleId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "RevokeRole"; }
    }

    /**
    * Identity
    */
    // @Route("/identity/roles/{RoleId}/activate", "POST")
    // @Api(Description="Identity")
    export class RoleActivate extends DomainCommand implements IReturn<CommandResponse>
    {
        roleId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "RoleActivate"; }
    }

    /**
    * Identity
    */
    // @Route("/identity/roles/{RoleId}/deactivate", "POST")
    // @Api(Description="Identity")
    export class RoleDeactivate extends DomainCommand implements IReturn<CommandResponse>
    {
        roleId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "RoleDeactivate"; }
    }

    /**
    * Identity
    */
    // @Route("/identity/roles", "POST")
    // @Api(Description="Identity")
    export class RoleDefine extends DomainCommand implements IReturn<CommandResponse>
    {
        roleId: string;
        name: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "RoleDefine"; }
    }

    /**
    * Identity
    */
    // @Route("/identity/roles/{RoleId}", "DELETE")
    // @Api(Description="Identity")
    export class RoleDestroy extends DomainCommand implements IReturn<CommandResponse>
    {
        roleId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "RoleDestroy"; }
    }

    /**
    * Identity
    */
    // @Route("/identity/roles/{RoleId}/revoke", "POST")
    // @Api(Description="Identity")
    export class RoleRevoke extends DomainCommand implements IReturn<CommandResponse>
    {
        roleId: string;
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "RoleRevoke"; }
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

    /**
    * Configuration
    */
    // @Route("/configuration/status", "GET")
    // @Api(Description="Configuration")
    export class GetStatus extends Query<Status> implements IReturn<QueryResponse<Status>>
    {
        createResponse() { return new QueryResponse<Status>(); }
        getTypeName() { return "GetStatus"; }
    }

    /**
    * Configuration
    */
    // @Route("/configuration/setup/seed", "POST")
    // @Api(Description="Configuration")
    export class Seed extends DomainCommand implements IReturn<CommandResponse>
    {
        createResponse() { return new CommandResponse(); }
        getTypeName() { return "Seed"; }
    }

}
