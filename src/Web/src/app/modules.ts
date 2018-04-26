import { CatalogModule } from './parts/catalog/catalogModule';

import { Store, StoreType } from './stores';

export interface Modules {
  catalog: CatalogModule;
}

export function createModules(store: StoreType) {

  return {
    catalog: new CatalogModule(store.catalog)
  };
}
