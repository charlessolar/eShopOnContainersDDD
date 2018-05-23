import { AdministrationModule } from './parts/administration/administrationModule';
import { AuthModule } from './parts/auth/authModule';
import { BasketModule } from './parts/basket/basketModule';
import { CatalogModule } from './parts/catalog/catalogModule';
import { ConfigurationModule } from './parts/configuration/configurationModule';
import { OrdersModule } from './parts/orders/ordersModule';
import { StoreType } from './stores';

export interface Modules {
  catalog: CatalogModule;
  configuration: ConfigurationModule;
  auth: AuthModule;
  administrate: AdministrationModule;
  basket: BasketModule;
  orders: OrdersModule;
}

export function createModules(store: StoreType) {

  return {
    catalog: new CatalogModule(),
    configuration: new ConfigurationModule(store),
    auth: new AuthModule(store),
    administrate: new AdministrationModule(store),
    basket: new BasketModule(store),
    orders: new OrdersModule(store)
  };
}
