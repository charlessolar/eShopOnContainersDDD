type UniversalRouterKey = {
  name: string | number,
  prefix?: string,
  delimiter?: string,
  optional: boolean,
  repeat: boolean,
  partial: boolean,
  pattern?: string
};

type UniversalRouterContext = {
  router: UniversalRouterType<*, *, *, *>,
  route: UniversalRouterRoute,
  next: (all?: boolean) => Promise<UniversalRouterRoute>,
  pathname: string,
  baseUrl: string,
  path: string,
  params: any,
  keys: Array<UniversalRouterKey>
};

type UniversalRouterRoute = {
  path?: string,
  name?: string,
  parent?: UniversalRouterRoute,
  children?: Array<UniversalRouterRoute>,
  action?: (context: UniversalRouterContext, params: any) => void;
  component?: (context: UniversalRouterContext) => any
};

type UniversalRouterResolveRoute<R, C, O> = (context: C, params: any) => O;

type UniversalRouterOptions<R, C, O> = {
  context?: C,
  baseUrl?: string,
  resolveRoute?: UniversalRouterResolveRoute<R, C, O>
};

type UniversalRouterResolveParams = {
  pathname: string
};

declare module "universal-router" {
  export default class UniversalRouterType<
    R, // Route type
    C, // Context type
    O, // Resolve output type
    RP extends UniversalRouterResolveParams = UniversalRouterResolveParams
  > {
    constructor(
      routes: R | Array<R>,
      options?: UniversalRouterOptions<R, C, O>
    ): UniversalRouterType<R, C, O, RP>;
    resolve(params: RP | string): Promise<O>;
    root: R;
  }
}

type UniversalRouterGenerateUrl = (
  routeName: string,
  routeParams?: Object
) => string;
type UniversalRouterGenerateUrlsFnOpts = {
  encode?: (str: string) => string,
  stringifyQueryParams?: (params: Object) => string
};
type UniversalRouterGenerateUrlsFn = (
  router: UniversalRouterType<*, *, *, *>,
  options?: UniversalRouterGenerateUrlsFnOpts
) => UniversalRouterGenerateUrl;

declare module "universal-router/generateUrls" {
  export default UniversalRouterGenerateUrlsFn = (
    router: UniversalRouterType<*, *, *, *>,
    options?: UniversalRouterGenerateUrlsFnOpts
  ) => UniversalRouterGenerateUrl;
}
