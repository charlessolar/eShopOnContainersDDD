import { CatalogModule } from './parts/catalog/catalogModule';
import { ConfigurationModule } from './parts/configuration/configurationModule';

import { Store, StoreType } from './stores';

export interface Modules {
  catalog: CatalogModule;
  configuration: ConfigurationModule;
}

export function createModules(store: StoreType) {

  return {
    catalog: new CatalogModule(),
    configuration: new ConfigurationModule(store)
  };
}
