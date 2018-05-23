/* tslint:disable */
/* Options:
Date: 2018-05-23 05:58:12
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

export namespace DTOs {

    export interface IReturn<T> {
        createResponse(): T;
    }

    export interface IReturnVoid {
        createResponse(): void;
    }

    export interface IPost {
    }

    // @DataContract
    export class ResponseError {
        // @DataMember(Order=1, EmitDefaultValue=false)
        public errorCode: string;

        // @DataMember(Order=2, EmitDefaultValue=false)
        public fieldName: string;

        // @DataMember(Order=3, EmitDefaultValue=false)
        public message: string;

        // @DataMember(Order=4, EmitDefaultValue=false)
        public meta: { [index: string]: string; };
    }

    // @DataContract
    export class ResponseStatus {
        // @DataMember(Order=1)
        public errorCode: string;

        // @DataMember(Order=2)
        public message: string;

        // @DataMember(Order=3)
        public stackTrace: string;

        // @DataMember(Order=4)
        public errors: ResponseError[];

        // @DataMember(Order=5)
        public meta: { [index: string]: string; };
    }

    export class Query<T> {
    }

    export class Basket {
        public id: string;
        public customerId: string;
        public customer: string;
        public totalItems: number;
        public totalQuantity: number;
        public subTotal: number;
        public created: number;
        public updated: number;
    }

    export class Paged<T> {
    }

    export class BasketIndex {
        public id: string;
        public customerId: string;
        public customer: string;
        public totalItems: number;
        public totalQuantity: number;
        public subTotal: number;
        public created: number;
        public updated: number;
    }

    export class CommandResponse {
        public roundTripMs: number;
        public responseStatus: ResponseStatus;
    }

    export class DomainCommand {
    }

    export class BasketItemIndex {
        public id: string;
        public basketId: string;
        public productId: string;
        public productPictureContents: string;
        public productPictureContentType: string;
        public productName: string;
        public productDescription: string;
        public productPrice: number;
        public quantity: number;
        public subTotal: number;
    }

    export class CatalogBrand {
        public id: string;
        public brand: string;
    }

    export class CatalogType {
        public id: string;
        public type: string;
    }

    export class CatalogProduct {
        public id: string;
        public name: string;
        public description: string;
        public price: number;
        public catalogTypeId: string;
        public catalogType: string;
        public catalogBrandId: string;
        public catalogBrand: string;
        public availableStock: number;
        public restockThreshold: number;
        public maxStockThreshold: number;
        public onReorder: boolean;
        public pictureContents: string;
        public pictureContentType: string;
    }

    export class CatalogProductIndex {
        public id: string;
        public name: string;
        public description: string;
        public price: number;
        public catalogTypeId: string;
        public catalogType: string;
        public catalogBrandId: string;
        public catalogBrand: string;
        public availableStock: number;
        public restockThreshold: number;
        public maxStockThreshold: number;
        public onReorder: boolean;
        public pictureContents: string;
        public pictureContentType: string;
    }

    export class User {
        public id: string;
        public givenName: string;
        public disabled: boolean;
        public roles: string[];
        public lastLogin: number;
    }

    export class StampedCommand {
        public stamp: number;
    }

    export class Location extends StampedCommand {
        public id: string;
        public code: string;
        public description: string;
        public points: Point[];
    }

    export class Campaign {
        public id: string;
        public name: string;
        public description: string;
        public start: string;
        public end: string;
        public pictureContents: string;
        public pictureContentType: string;
    }

    export class OrderingBuyerIndex {
        public id: string;
        public givenName: string;
        public goodStanding: boolean;
        public totalSpent: number;
        public preferredCity: string;
        public preferredState: string;
        public preferredCountry: string;
        public preferredZipCode: string;
        public preferredPaymentCardholder: string;
        public preferredPaymentMethod: string;
        public preferredPaymentExpiration: string;
    }

    export class OrderingBuyer {
        public id: string;
        public givenName: string;
        public goodStanding: boolean;
        public preferredAddressId: string;
        public preferredPaymentMethodId: string;
    }

    export class Address {
        public id: string;
        public userName: string;
        public alias: string;
        public street: string;
        public city: string;
        public state: string;
        public country: string;
        public zipCode: string;
    }

    export class PaymentMethod {
        public id: string;
        public userName: string;
        public alias: string;
        public cardNumber: string;
        public securityNumber: string;
        public cardholderName: string;
        public expiration: string;
        public cardType: string;
    }

    export class OrderingOrder {
        public id: string;
        public status: string;
        public statusDescription: string;
        public userName: string;
        public buyerName: string;
        public shippingAddressId: string;
        public shippingAddress: string;
        public shippingCityState: string;
        public shippingZipCode: string;
        public shippingCountry: string;
        public billingAddressId: string;
        public billingAddress: string;
        public billingCityState: string;
        public billingZipCode: string;
        public billingCountry: string;
        public paymentMethodId: string;
        public paymentMethod: string;
        public totalItems: number;
        public totalQuantity: number;
        public subTotal: number;
        public additionalFees: number;
        public additionalTaxes: number;
        public total: number;
        public created: number;
        public updated: number;
        public paid: boolean;
        public items: OrderingOrderItem[];
    }

    export class OrderingOrderIndex {
        public id: string;
        public status: string;
        public statusDescription: string;
        public userName: string;
        public buyerName: string;
        public shippingAddressId: string;
        public shippingAddress: string;
        public shippingCity: string;
        public shippingState: string;
        public shippingZipCode: string;
        public shippingCountry: string;
        public billingAddressId: string;
        public billingAddress: string;
        public billingCity: string;
        public billingState: string;
        public billingZipCode: string;
        public billingCountry: string;
        public paymentMethodId: string;
        public paymentMethod: string;
        public totalItems: number;
        public totalQuantity: number;
        public subTotal: number;
        public additional: number;
        public total: number;
        public created: number;
        public updated: number;
        public paid: boolean;
    }

    export class SalesWeekOverWeek {
        public id: string;
        public relevancy: number;
        public dayOfWeek: string;
        public value: number;
    }

    export class SalesByState {
        public id: string;
        public relevancy: number;
        public state: string;
        public value: number;
    }

    export class SalesChart {
        public id: string;
        public relevancy: number;
        public label: string;
        public value: number;
    }

    export class OrderingOrderItem {
        public id: string;
        public orderId: string;
        public productId: string;
        public productPictureContents: string;
        public productPictureContentType: string;
        public productName: string;
        public productDescription: string;
        public productPrice: number;
        public price: number;
        public quantity: number;
        public subTotal: number;
        public additionalFees: number;
        public additionalTaxes: number;
        public total: number;
    }

    export class ConfigurationStatus {
        public id: string;
        public isSetup: boolean;
        public setupContexts: string[];
    }

    export class Point {
        public id: string;
        public locationId: string;
        public longitude: number;
        public latitude: number;
    }

    export interface ICommand {
    }

    export interface IMessage {
    }

    // @DataContract
    export class AuthenticateResponse {
        // @DataMember(Order=1)
        public userId: string;

        // @DataMember(Order=2)
        public sessionId: string;

        // @DataMember(Order=3)
        public userName: string;

        // @DataMember(Order=4)
        public displayName: string;

        // @DataMember(Order=5)
        public referrerUrl: string;

        // @DataMember(Order=6)
        public bearerToken: string;

        // @DataMember(Order=7)
        public refreshToken: string;

        // @DataMember(Order=8)
        public responseStatus: ResponseStatus;

        // @DataMember(Order=9)
        public meta: { [index: string]: string; };
    }

    // @DataContract
    export class AssignRolesResponse {
        // @DataMember(Order=1)
        public allRoles: string[];

        // @DataMember(Order=2)
        public allPermissions: string[];

        // @DataMember(Order=3)
        public responseStatus: ResponseStatus;
    }

    // @DataContract
    export class UnAssignRolesResponse {
        // @DataMember(Order=1)
        public allRoles: string[];

        // @DataMember(Order=2)
        public allPermissions: string[];

        // @DataMember(Order=3)
        public responseStatus: ResponseStatus;
    }

    // @DataContract
    export class ConvertSessionToTokenResponse {
        // @DataMember(Order=1)
        public meta: { [index: string]: string; };

        // @DataMember(Order=2)
        public accessToken: string;

        // @DataMember(Order=3)
        public responseStatus: ResponseStatus;
    }

    // @DataContract
    export class GetAccessTokenResponse {
        // @DataMember(Order=1)
        public accessToken: string;

        // @DataMember(Order=2)
        public responseStatus: ResponseStatus;
    }

    // @DataContract
    export class RegisterResponse {
        // @DataMember(Order=1)
        public userId: string;

        // @DataMember(Order=2)
        public sessionId: string;

        // @DataMember(Order=3)
        public userName: string;

        // @DataMember(Order=4)
        public referrerUrl: string;

        // @DataMember(Order=5)
        public bearerToken: string;

        // @DataMember(Order=6)
        public refreshToken: string;

        // @DataMember(Order=7)
        public responseStatus: ResponseStatus;

        // @DataMember(Order=8)
        public meta: { [index: string]: string; };
    }

    export class QueryResponse<T> {
        public roundTripMs: number;
        public payload: T;
        public responseStatus: ResponseStatus;
    }

    export class PagedResponse<T> {
        public roundTripMs: number;
        public total: number;
        public records: T[];
        public responseStatus: ResponseStatus;
    }

    // @Route("/auth")
    // @Route("/auth/{provider}")
    // @Route("/authenticate")
    // @Route("/authenticate/{provider}")
    // @DataContract
    export class Authenticate implements IReturn<AuthenticateResponse>, IPost {
        // @DataMember(Order=1)
        public provider: string;

        // @DataMember(Order=2)
        public state: string;

        // @DataMember(Order=3)
        public oauth_token: string;

        // @DataMember(Order=4)
        public oauth_verifier: string;

        // @DataMember(Order=5)
        public userName: string;

        // @DataMember(Order=6)
        public password: string;

        // @DataMember(Order=7)
        public rememberMe: boolean;

        // @DataMember(Order=8)
        public continue: string;

        // @DataMember(Order=9)
        public nonce: string;

        // @DataMember(Order=10)
        public uri: string;

        // @DataMember(Order=11)
        public response: string;

        // @DataMember(Order=12)
        public qop: string;

        // @DataMember(Order=13)
        public nc: string;

        // @DataMember(Order=14)
        public cnonce: string;

        // @DataMember(Order=15)
        public useTokenCookie: boolean;

        // @DataMember(Order=16)
        public accessToken: string;

        // @DataMember(Order=17)
        public accessTokenSecret: string;

        // @DataMember(Order=18)
        public meta: { [index: string]: string; };
        public createResponse() { return new AuthenticateResponse(); }
        public getTypeName() { return 'Authenticate'; }
    }

    // @Route("/assignroles")
    // @DataContract
    export class AssignRoles implements IReturn<AssignRolesResponse>, IPost {
        // @DataMember(Order=1)
        public userName: string;

        // @DataMember(Order=2)
        public permissions: string[];

        // @DataMember(Order=3)
        public roles: string[];
        public createResponse() { return new AssignRolesResponse(); }
        public getTypeName() { return 'AssignRoles'; }
    }

    // @Route("/unassignroles")
    // @DataContract
    export class UnAssignRoles implements IReturn<UnAssignRolesResponse>, IPost {
        // @DataMember(Order=1)
        public userName: string;

        // @DataMember(Order=2)
        public permissions: string[];

        // @DataMember(Order=3)
        public roles: string[];
        public createResponse() { return new UnAssignRolesResponse(); }
        public getTypeName() { return 'UnAssignRoles'; }
    }

    // @Route("/session-to-token")
    // @DataContract
    export class ConvertSessionToToken implements IReturn<ConvertSessionToTokenResponse>, IPost {
        // @DataMember(Order=1)
        public preserveSession: boolean;
        public createResponse() { return new ConvertSessionToTokenResponse(); }
        public getTypeName() { return 'ConvertSessionToToken'; }
    }

    // @Route("/access-token")
    // @DataContract
    export class GetAccessToken implements IReturn<GetAccessTokenResponse>, IPost {
        // @DataMember(Order=1)
        public refreshToken: string;
        public createResponse() { return new GetAccessTokenResponse(); }
        public getTypeName() { return 'GetAccessToken'; }
    }

    // @Route("/register")
    // @DataContract
    export class Register implements IReturn<RegisterResponse>, IPost {
        // @DataMember(Order=1)
        public userName: string;

        // @DataMember(Order=2)
        public firstName: string;

        // @DataMember(Order=3)
        public lastName: string;

        // @DataMember(Order=4)
        public displayName: string;

        // @DataMember(Order=5)
        public email: string;

        // @DataMember(Order=6)
        public password: string;

        // @DataMember(Order=7)
        public autoLogin: boolean;

        // @DataMember(Order=8)
        public continue: string;
        public createResponse() { return new RegisterResponse(); }
        public getTypeName() { return 'Register'; }
    }

    /**
    * Basket
    */
    // @Route("/basket", "GET")
    // @Api(Description="Basket")
    export class GetBasket extends Query<Basket> implements IReturn<QueryResponse<Basket>> {
        public basketId: string;
        public createResponse() { return new QueryResponse<Basket>(); }
        public getTypeName() { return 'GetBasket'; }
    }

    /**
    * Basket
    */
    // @Route("/basket/list", "GET")
    // @Api(Description="Basket")
    export class ListBaskets extends Paged<BasketIndex> implements IReturn<PagedResponse<BasketIndex>> {
        public createResponse() { return new PagedResponse<BasketIndex>(); }
        public getTypeName() { return 'ListBaskets'; }
    }

    /**
    * Basket
    */
    // @Route("/basket", "POST")
    // @Api(Description="Basket")
    export class InitiateBasket extends DomainCommand implements IReturn<CommandResponse> {
        public basketId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'InitiateBasket'; }
    }

    /**
    * Basket
    */
    // @Route("/basket/claim", "POST")
    // @Api(Description="Basket")
    export class ClaimBasket extends DomainCommand implements IReturn<CommandResponse> {
        public basketId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'ClaimBasket'; }
    }

    /**
    * Basket
    */
    // @Route("/basket", "DELETE")
    // @Api(Description="Basket")
    export class BasketDestroy extends DomainCommand implements IReturn<CommandResponse> {
        public basketId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'BasketDestroy'; }
    }

    /**
    * Basket
    */
    // @Route("/basket/item", "GET")
    // @Api(Description="Basket")
    export class GetBasketItems extends Paged<BasketItemIndex> implements IReturn<PagedResponse<BasketItemIndex>> {
        public basketId: string;
        public createResponse() { return new PagedResponse<BasketItemIndex>(); }
        public getTypeName() { return 'GetBasketItems'; }
    }

    /**
    * Basket
    */
    // @Route("/basket/item", "POST")
    // @Api(Description="Basket")
    export class AddBasketItem extends DomainCommand implements IReturn<CommandResponse> {
        public basketId: string;
        public productId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'AddBasketItem'; }
    }

    /**
    * Basket
    */
    // @Route("/basket/item/{ProductId}", "DELETE")
    // @Api(Description="Basket")
    export class RemoveBasketItem extends DomainCommand implements IReturn<CommandResponse> {
        public basketId: string;
        public productId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'RemoveBasketItem'; }
    }

    /**
    * Basket
    */
    // @Route("/basket/item/{ProductId}/quantity", "POST")
    // @Api(Description="Basket")
    export class UpdateBasketItemQuantity extends DomainCommand implements IReturn<CommandResponse> {
        public basketId: string;
        public productId: string;
        public quantity: number;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'UpdateBasketItemQuantity'; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/brand", "GET")
    // @Api(Description="Catalog")
    export class ListCatalogBrands extends Paged<CatalogBrand> implements IReturn<PagedResponse<CatalogBrand>> {
        public term: string;
        public limit: number;
        public id: string;
        public createResponse() { return new PagedResponse<CatalogBrand>(); }
        public getTypeName() { return 'ListCatalogBrands'; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/brand", "POST")
    // @Api(Description="Catalog")
    export class AddCatalogBrand extends DomainCommand implements IReturn<CommandResponse> {
        public brandId: string;
        public brand: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'AddCatalogBrand'; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/brand/{BrandId}", "DELETE")
    // @Api(Description="Catalog")
    export class RemoveCatalogBrand extends DomainCommand implements IReturn<CommandResponse> {
        public brandId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'RemoveCatalogBrand'; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/type", "GET")
    // @Api(Description="Catalog")
    export class ListCatalogTypes extends Paged<CatalogType> implements IReturn<PagedResponse<CatalogType>> {
        public term: string;
        public limit: number;
        public id: string;
        public createResponse() { return new PagedResponse<CatalogType>(); }
        public getTypeName() { return 'ListCatalogTypes'; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/type", "POST")
    // @Api(Description="Catalog")
    export class AddCatalogType extends DomainCommand implements IReturn<CommandResponse> {
        public typeId: string;
        public type: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'AddCatalogType'; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/type/{TypeId}", "POST")
    // @Api(Description="Catalog")
    export class RemoveCatalogType extends DomainCommand implements IReturn<CommandResponse> {
        public typeId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'RemoveCatalogType'; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/products/{ProductId}", "GET")
    // @Api(Description="Catalog")
    export class GetProduct extends Query<CatalogProduct> implements IReturn<QueryResponse<CatalogProduct>> {
        public productId: string;
        public createResponse() { return new QueryResponse<CatalogProduct>(); }
        public getTypeName() { return 'GetProduct'; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/products", "GET")
    // @Api(Description="Catalog")
    export class ListProducts extends Paged<CatalogProductIndex> implements IReturn<PagedResponse<CatalogProductIndex>> {
        public createResponse() { return new PagedResponse<CatalogProductIndex>(); }
        public getTypeName() { return 'ListProducts'; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog", "GET")
    // @Api(Description="Catalog")
    export class Catalog extends Paged<CatalogProductIndex> implements IReturn<PagedResponse<CatalogProductIndex>> {
        public brandId: string;
        public typeId: string;
        public search: string;
        public createResponse() { return new PagedResponse<CatalogProductIndex>(); }
        public getTypeName() { return 'Catalog'; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/products", "POST")
    // @Api(Description="Catalog")
    export class AddProduct extends DomainCommand implements IReturn<CommandResponse> {
        public productId: string;
        public name: string;
        public price: number;
        public catalogBrandId: string;
        public catalogTypeId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'AddProduct'; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/products/{ProductId}", "DELETE")
    // @Api(Description="Catalog")
    export class RemoveProduct extends DomainCommand implements IReturn<CommandResponse> {
        public productId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'RemoveProduct'; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/products/{ProductId}/picture", "POST")
    // @Api(Description="Catalog")
    export class SetPictureProduct extends DomainCommand implements IReturn<CommandResponse> {
        public productId: string;
        public content: string;
        public contentType: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'SetPictureProduct'; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/products/{ProductId}/description", "POST")
    // @Api(Description="Catalog")
    export class UpdateDescriptionProduct extends DomainCommand implements IReturn<CommandResponse> {
        public productId: string;
        public description: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'UpdateDescriptionProduct'; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/products/{ProductId}/mark", "POST")
    // @Api(Description="Catalog")
    export class MarkReordered extends DomainCommand implements IReturn<CommandResponse> {
        public productId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'MarkReordered'; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/products/{ProductId}/unmark", "POST")
    // @Api(Description="Catalog")
    export class UnMarkReordered extends DomainCommand implements IReturn<CommandResponse> {
        public productId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'UnMarkReordered'; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/products/{ProductId}/stock", "POST")
    // @Api(Description="Catalog")
    export class UpdateStock extends DomainCommand implements IReturn<CommandResponse> {
        public productId: string;
        public stock: number;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'UpdateStock'; }
    }

    /**
    * Catalog
    */
    // @Route("/catalog/products/{ProductId}/price", "POST")
    // @Api(Description="Catalog")
    export class UpdatePriceProduct extends DomainCommand implements IReturn<CommandResponse> {
        public productId: string;
        public price: number;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'UpdatePriceProduct'; }
    }

    /**
    * Identity
    */
    // @Route("/identity/user", "GET")
    // @Api(Description="Identity")
    export class GetIdentity extends Query<User> implements IReturn<QueryResponse<User>> {
        public createResponse() { return new QueryResponse<User>(); }
        public getTypeName() { return 'GetIdentity'; }
    }

    /**
    * Identity
    */
    // @Route("/identity/users", "GET")
    // @Api(Description="Identity")
    export class GetUsers extends Paged<User> implements IReturn<PagedResponse<User>> {
        public createResponse() { return new PagedResponse<User>(); }
        public getTypeName() { return 'GetUsers'; }
    }

    /**
    * Identity
    */
    // @Route("/identity/users", "POST")
    // @Api(Description="Identity")
    export class UserRegister extends DomainCommand implements IReturn<CommandResponse> {
        public givenName: string;
        public userName: string;
        public password: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'UserRegister'; }
    }

    /**
    * Identity
    */
    // @Route("/identity/users/{UserName}/name", "POST")
    // @Api(Description="Identity")
    export class ChangeName extends DomainCommand implements IReturn<CommandResponse> {
        public userName: string;
        public givenName: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'ChangeName'; }
    }

    /**
    * Identity
    */
    // @Route("/identity/users/{UserName}/password", "POST")
    // @Api(Description="Identity")
    export class ChangePassword extends DomainCommand implements IReturn<CommandResponse> {
        public userName: string;
        public password: string;
        public newPassword: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'ChangePassword'; }
    }

    /**
    * Identity
    */
    // @Route("/identity/users/{UserName}/enable", "POST")
    // @Api(Description="Identity")
    export class UserEnable extends DomainCommand implements IReturn<CommandResponse> {
        public userName: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'UserEnable'; }
    }

    /**
    * Identity
    */
    // @Route("/identity/users/{UserName}/disable", "POST")
    // @Api(Description="Identity")
    export class UserDisable extends DomainCommand implements IReturn<CommandResponse> {
        public userName: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'UserDisable'; }
    }

    /**
    * Identity
    */
    // @Route("/identity/users/{UserName}/assign", "POST")
    // @Api(Description="Identity")
    export class AssignRole extends DomainCommand implements IReturn<CommandResponse> {
        public userName: string;
        public roleId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'AssignRole'; }
    }

    /**
    * Identity
    */
    // @Route("/identity/users/{UserName}/revoke", "POST")
    // @Api(Description="Identity")
    export class RevokeRole extends DomainCommand implements IReturn<CommandResponse> {
        public userName: string;
        public roleId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'RevokeRole'; }
    }

    /**
    * Identity
    */
    // @Route("/identity/roles/{RoleId}/activate", "POST")
    // @Api(Description="Identity")
    export class RoleActivate extends DomainCommand implements IReturn<CommandResponse> {
        public roleId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'RoleActivate'; }
    }

    /**
    * Identity
    */
    // @Route("/identity/roles/{RoleId}/deactivate", "POST")
    // @Api(Description="Identity")
    export class RoleDeactivate extends DomainCommand implements IReturn<CommandResponse> {
        public roleId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'RoleDeactivate'; }
    }

    /**
    * Identity
    */
    // @Route("/identity/roles", "POST")
    // @Api(Description="Identity")
    export class RoleDefine extends DomainCommand implements IReturn<CommandResponse> {
        public roleId: string;
        public name: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'RoleDefine'; }
    }

    /**
    * Identity
    */
    // @Route("/identity/roles/{RoleId}", "DELETE")
    // @Api(Description="Identity")
    export class RoleDestroy extends DomainCommand implements IReturn<CommandResponse> {
        public roleId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'RoleDestroy'; }
    }

    /**
    * Identity
    */
    // @Route("/identity/roles/{RoleId}/revoke", "POST")
    // @Api(Description="Identity")
    export class RoleRevoke extends DomainCommand implements IReturn<CommandResponse> {
        public roleId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'RoleRevoke'; }
    }

    /**
    * Location
    */
    // @Route("/location", "GET")
    // @Api(Description="Location")
    export class ListLocations extends Paged<Location> implements IReturn<PagedResponse<Location>> {
        public createResponse() { return new PagedResponse<Location>(); }
        public getTypeName() { return 'ListLocations'; }
    }

    /**
    * Location
    */
    // @Route("/location", "POST")
    // @Api(Description="Location")
    export class AddLocation extends DomainCommand implements IReturn<CommandResponse> {
        public locationId: string;
        public code: string;
        public description: string;
        public parentId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'AddLocation'; }
    }

    /**
    * Location
    */
    // @Route("/location/{LocationId}", "DELETE")
    // @Api(Description="Location")
    export class RemoveLocation extends DomainCommand implements IReturn<CommandResponse> {
        public locationId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'RemoveLocation'; }
    }

    /**
    * Location
    */
    // @Route("/location/{LocationId}/description", "POST")
    // @Api(Description="Location")
    export class UpdateDescriptionLocation extends DomainCommand implements IReturn<CommandResponse> {
        public locationId: string;
        public description: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'UpdateDescriptionLocation'; }
    }

    /**
    * Location
    */
    // @Route("/location/{LocationId}/point", "POST")
    // @Api(Description="Location")
    export class AddPoint extends DomainCommand implements IReturn<CommandResponse> {
        public locationId: string;
        public pointId: string;
        public longitude: number;
        public latitude: number;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'AddPoint'; }
    }

    /**
    * Location
    */
    // @Route("/location/{LocationId}/point/{PointId}", "DELETE")
    // @Api(Description="Location")
    export class RemovePoint extends DomainCommand implements IReturn<CommandResponse> {
        public locationId: string;
        public pointId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'RemovePoint'; }
    }

    export class GetCampaign extends Query<Campaign> implements IReturn<QueryResponse<Campaign>> {
        public campaignId: string;
        public createResponse() { return new QueryResponse<Campaign>(); }
        public getTypeName() { return 'GetCampaign'; }
    }

    export class ListCampaigns extends Paged<Campaign> implements IReturn<PagedResponse<Campaign>> {
        public createResponse() { return new PagedResponse<Campaign>(); }
        public getTypeName() { return 'ListCampaigns'; }
    }

    /**
    * Marketing
    */
    // @Route("/campaign/{CampaignId}/description", "POST")
    // @Api(Description="Marketing")
    export class ChangeDescriptionCampaign extends DomainCommand implements IReturn<CommandResponse> {
        public campaignId: string;
        public description: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'ChangeDescriptionCampaign'; }
    }

    /**
    * Marketing
    */
    // @Route("/campaign", "POST")
    // @Api(Description="Marketing")
    export class DefineCampaign extends DomainCommand implements IReturn<CommandResponse> {
        public campaignId: string;
        public name: string;
        public description: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'DefineCampaign'; }
    }

    /**
    * Marketing
    */
    // @Route("/campaign/{CampaignId}/period", "POST")
    // @Api(Description="Marketing")
    export class SetPeriodCampaign extends DomainCommand implements IReturn<CommandResponse> {
        public campaignId: string;
        public start: string;
        public end: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'SetPeriodCampaign'; }
    }

    /**
    * Marketing
    */
    // @Route("/campaign/{CampaignId}/picture", "POST")
    // @Api(Description="Marketing")
    export class SetPictureCampaign extends DomainCommand implements IReturn<CommandResponse> {
        public campaignId: string;
        public pictureUrl: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'SetPictureCampaign'; }
    }

    /**
    * Marketing
    */
    // @Route("/campaign/{CampaignId}/rule", "POST")
    // @Api(Description="Marketing")
    export class AddCampaignRule extends DomainCommand implements IReturn<CommandResponse> {
        public campaignId: string;
        public ruleId: string;
        public description: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'AddCampaignRule'; }
    }

    /**
    * Marketing
    */
    // @Route("/campaign/{CampaignId}/rule/{RuleId}", "DELETE")
    // @Api(Description="Marketing")
    export class RemoveCampaignRule extends DomainCommand implements IReturn<CommandResponse> {
        public campaignId: string;
        public ruleId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'RemoveCampaignRule'; }
    }

    /**
    * Ordering
    */
    // @Route("/buyers", "GET")
    // @Api(Description="Ordering")
    export class Buyers extends Paged<OrderingBuyerIndex> implements IReturn<PagedResponse<OrderingBuyerIndex>> {
        public createResponse() { return new PagedResponse<OrderingBuyerIndex>(); }
        public getTypeName() { return 'Buyers'; }
    }

    /**
    * Ordering
    */
    // @Route("/buyer", "GET")
    // @Api(Description="Ordering")
    export class Buyer extends Query<OrderingBuyer> implements IReturn<QueryResponse<OrderingBuyer>> {
        public userName: string;
        public createResponse() { return new QueryResponse<OrderingBuyer>(); }
        public getTypeName() { return 'Buyer'; }
    }

    /**
    * Ordering
    */
    // @Route("/buyer", "POST")
    // @Api(Description="Ordering")
    export class InitiateBuyer extends DomainCommand implements IReturn<CommandResponse> {
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'InitiateBuyer'; }
    }

    /**
    * Ordering
    */
    // @Route("/buyer/{UserName}/good", "POST")
    // @Api(Description="Ordering")
    export class MarkGoodStanding extends DomainCommand implements IReturn<CommandResponse> {
        public userName: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'MarkGoodStanding'; }
    }

    /**
    * Ordering
    */
    // @Route("/buyer/{UserName}/suspend", "POST")
    // @Api(Description="Ordering")
    export class MarkSuspended extends DomainCommand implements IReturn<CommandResponse> {
        public userName: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'MarkSuspended'; }
    }

    /**
    * Ordering
    */
    // @Route("/buyer/{UserName}/preferred_address", "POST")
    // @Api(Description="Ordering")
    export class SetPreferredAddress extends DomainCommand implements IReturn<CommandResponse> {
        public addressId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'SetPreferredAddress'; }
    }

    /**
    * Ordering
    */
    // @Route("/buyer/{UserName}/preferred_payment", "POST")
    // @Api(Description="Ordering")
    export class SetPreferredPaymentMethod extends DomainCommand implements IReturn<CommandResponse> {
        public paymentMethodId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'SetPreferredPaymentMethod'; }
    }

    /**
    * Ordering
    */
    // @Route("/buyer/address", "GET")
    // @Api(Description="Ordering")
    export class ListAddresses extends Paged<Address> implements IReturn<PagedResponse<Address>> {
        public term: string;
        public id: string;
        public createResponse() { return new PagedResponse<Address>(); }
        public getTypeName() { return 'ListAddresses'; }
    }

    /**
    * Ordering
    */
    // @Route("/buyer/address", "POST")
    // @Api(Description="Ordering")
    export class AddBuyerAddress extends DomainCommand implements IReturn<CommandResponse> {
        public addressId: string;
        public alias: string;
        public street: string;
        public city: string;
        public state: string;
        public country: string;
        public zipCode: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'AddBuyerAddress'; }
    }

    /**
    * Ordering
    */
    // @Route("/buyer/address/{AddressId}", "DELETE")
    // @Api(Description="Ordering")
    export class RemoveBuyerAddress extends DomainCommand implements IReturn<CommandResponse> {
        public addressId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'RemoveBuyerAddress'; }
    }

    /**
    * Ordering
    */
    // @Route("/buyer/payment_method", "GET")
    // @Api(Description="Ordering")
    export class ListPaymentMethods extends Paged<PaymentMethod> implements IReturn<PagedResponse<PaymentMethod>> {
        public term: string;
        public id: string;
        public createResponse() { return new PagedResponse<PaymentMethod>(); }
        public getTypeName() { return 'ListPaymentMethods'; }
    }

    /**
    * Ordering
    */
    // @Route("/buyer/payment_method", "POST")
    // @Api(Description="Ordering")
    export class AddBuyerPaymentMethod extends DomainCommand implements IReturn<CommandResponse> {
        public paymentMethodId: string;
        public alias: string;
        public cardNumber: string;
        public securityNumber: string;
        public cardholderName: string;
        public expiration: string;
        public cardType: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'AddBuyerPaymentMethod'; }
    }

    /**
    * Ordering
    */
    // @Route("/buyer/payment_method/{PaymentMethodId}", "DELETE")
    // @Api(Description="Ordering")
    export class RemoveBuyerPaymentMethod extends DomainCommand implements IReturn<CommandResponse> {
        public paymentMethodId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'RemoveBuyerPaymentMethod'; }
    }

    /**
    * Ordering
    */
    // @Route("/order/{OrderId}", "GET")
    // @Api(Description="Ordering")
    export class GetOrder extends Query<OrderingOrder> implements IReturn<QueryResponse<OrderingOrder>> {
        public orderId: string;
        public createResponse() { return new QueryResponse<OrderingOrder>(); }
        public getTypeName() { return 'GetOrder'; }
    }

    /**
    * Ordering
    */
    // @Route("/order", "GET")
    // @Api(Description="Ordering")
    export class BuyerOrders extends Paged<OrderingOrderIndex> implements IReturn<PagedResponse<OrderingOrderIndex>> {
        public orderStatus: string;
        public from: string;
        public to: string;
        public createResponse() { return new PagedResponse<OrderingOrderIndex>(); }
        public getTypeName() { return 'BuyerOrders'; }
    }

    /**
    * Ordering
    */
    // @Route("/orders", "GET")
    // @Api(Description="Ordering")
    export class ListOrders extends Paged<OrderingOrderIndex> implements IReturn<PagedResponse<OrderingOrderIndex>> {
        public orderStatus: string;
        public from: string;
        public to: string;
        public createResponse() { return new PagedResponse<OrderingOrderIndex>(); }
        public getTypeName() { return 'ListOrders'; }
    }

    /**
    * Ordering
    */
    // @Route("/orders/sales_week_over_week", "GET")
    // @Api(Description="Ordering")
    export class OrderingSalesWeekOverWeek extends Paged<SalesWeekOverWeek> implements IReturn<PagedResponse<SalesWeekOverWeek>> {
        public from: string;
        public to: string;
        public createResponse() { return new PagedResponse<SalesWeekOverWeek>(); }
        public getTypeName() { return 'OrderingSalesWeekOverWeek'; }
    }

    /**
    * Ordering
    */
    // @Route("/orders/sales_by_state", "GET")
    // @Api(Description="Ordering")
    export class OrderingSalesByState extends Paged<SalesByState> implements IReturn<PagedResponse<SalesByState>> {
        public from: string;
        public to: string;
        public createResponse() { return new PagedResponse<SalesByState>(); }
        public getTypeName() { return 'OrderingSalesByState'; }
    }

    /**
    * Ordering
    */
    // @Route("/orders/sales", "GET")
    // @Api(Description="Ordering")
    export class OrderingSalesChart extends Paged<SalesChart> implements IReturn<PagedResponse<SalesChart>> {
        public from: string;
        public to: string;
        public createResponse() { return new PagedResponse<SalesChart>(); }
        public getTypeName() { return 'OrderingSalesChart'; }
    }

    /**
    * Ordering
    */
    // @Route("/order/{OrderId}/cancel", "POST")
    // @Api(Description="Ordering")
    export class CancelOrder extends DomainCommand implements IReturn<CommandResponse> {
        public orderId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'CancelOrder'; }
    }

    /**
    * Ordering
    */
    // @Route("/order/{OrderId}/confirm", "POST")
    // @Api(Description="Ordering")
    export class ConfirmOrder extends DomainCommand implements IReturn<CommandResponse> {
        public orderId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'ConfirmOrder'; }
    }

    /**
    * Ordering
    */
    // @Route("/order", "POST")
    // @Api(Description="Ordering")
    export class DraftOrder extends DomainCommand implements IReturn<CommandResponse> {
        public orderId: string;
        public basketId: string;
        public billingAddressId: string;
        public shippingAddressId: string;
        public paymentMethodId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'DraftOrder'; }
    }

    /**
    * Ordering
    */
    // @Route("/order/{OrderId}/pay", "POST")
    // @Api(Description="Ordering")
    export class PayOrder extends DomainCommand implements IReturn<CommandResponse> {
        public orderId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'PayOrder'; }
    }

    /**
    * Ordering
    */
    // @Route("/order/{OrderId}/ship", "POST")
    // @Api(Description="Ordering")
    export class ShipOrder extends DomainCommand implements IReturn<CommandResponse> {
        public orderId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'ShipOrder'; }
    }

    /**
    * Ordering
    */
    // @Route("/order/{OrderId}/address", "POST")
    // @Api(Description="Ordering")
    export class ChangeAddressOrder extends DomainCommand implements IReturn<CommandResponse> {
        public orderId: string;
        public shippingId: string;
        public billingId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'ChangeAddressOrder'; }
    }

    /**
    * Ordering
    */
    // @Route("/order/{OrderId}/payment_method", "POST")
    // @Api(Description="Ordering")
    export class ChangePaymentMethodOrder extends DomainCommand implements IReturn<CommandResponse> {
        public orderId: string;
        public paymentMethodId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'ChangePaymentMethodOrder'; }
    }

    /**
    * Ordering
    */
    // @Route("/order/{OrderId}/item", "GET")
    // @Api(Description="Ordering")
    export class ListOrderItems extends Paged<OrderingOrderItem> implements IReturn<PagedResponse<OrderingOrderItem>> {
        public orderId: string;
        public createResponse() { return new PagedResponse<OrderingOrderItem>(); }
        public getTypeName() { return 'ListOrderItems'; }
    }

    /**
    * Ordering
    */
    // @Route("/order/{OrderId}/item", "POST")
    // @Api(Description="Ordering")
    export class AddOrderItem extends DomainCommand implements IReturn<CommandResponse> {
        public productId: string;
        public orderId: string;
        public quantity: number;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'AddOrderItem'; }
    }

    /**
    * Ordering
    */
    // @Route("/order/{OrderId}/item/{ProductId}/price", "POST")
    // @Api(Description="Ordering")
    export class OverridePriceOrderItem extends DomainCommand implements IReturn<CommandResponse> {
        public productId: string;
        public orderId: string;
        public price: number;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'OverridePriceOrderItem'; }
    }

    /**
    * Ordering
    */
    // @Route("/order/{OrderId}/item/{ProductId}", "DELETE")
    // @Api(Description="Ordering")
    export class RemoveOrderItem extends DomainCommand implements IReturn<CommandResponse> {
        public productId: string;
        public orderId: string;
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'RemoveOrderItem'; }
    }

    /**
    * Configuration
    */
    // @Route("/configuration/status", "GET")
    // @Api(Description="Configuration")
    export class GetStatus extends Query<ConfigurationStatus> implements IReturn<QueryResponse<ConfigurationStatus>> {
        public createResponse() { return new QueryResponse<ConfigurationStatus>(); }
        public getTypeName() { return 'GetStatus'; }
    }

    /**
    * Configuration
    */
    // @Route("/configuration/setup/seed", "POST")
    // @Api(Description="Configuration")
    export class Seed extends DomainCommand implements IReturn<CommandResponse> {
        public createResponse() { return new CommandResponse(); }
        public getTypeName() { return 'Seed'; }
    }

}
