import { AuthModule } from './parts/auth/authModule';
import { CatalogModule } from './parts/catalog/catalogModule';
import { ConfigurationModule } from './parts/configuration/configurationModule';
import { AdministrationModule } from './parts/administration/administrationModule';

import { Store, StoreType } from './stores';

export interface Modules {
  catalog: CatalogModule;
  configuration: ConfigurationModule;
  auth: AuthModule;
  administrate: AdministrationModule;
}

export function createModules(store: StoreType) {

  return {
    catalog: new CatalogModule(),
    configuration: new ConfigurationModule(store),
    auth: new AuthModule(store),
    administrate: new AdministrationModule()
  };
}
