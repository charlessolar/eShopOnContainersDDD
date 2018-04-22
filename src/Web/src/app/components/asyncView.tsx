import * as React from 'react';
import { observable, action, runInAction } from 'mobx';
import { observer } from 'mobx-react';

import Fade from 'material-ui/transitions/Fade';
import { CircularProgress } from 'material-ui/Progress';

import { Store } from '../utils/store';
import { Context } from '../context';

interface AsyncViewProps {
  url?: string;
  component?: any;
  store?: Store;

  action?: Promise<{}>;
  getComponent?: (check?: number, cb?: (props: AsyncViewProps) => void) => Promise<any>;
  loading?: () => any;
}
interface AsyncViewState {
  componentData: any;
  error: any;
  loading: boolean;
}

class AsyncStore {
  constructor(private _context: Context) {
  }

  @observable
  public loading: boolean;
  @observable
  public error: boolean;
  @observable
  public componentData: any;

  private loadedStore: string;

  private waitingFor: number;

  @action
  public loadComponent(store?: Store, component?: any, getComponent?: (check?: number, cb?: (props: AsyncViewProps) => void) => Promise<any>) {
    // cheap hack to not reload async route unless store name changes
    if (this.loadedStore && store.constructor.name === this.loadedStore) {
      return;
    }
    this.loadedStore = store.constructor.name;

    if (store) {
      this.loading = true;
      store.fetch().then(action(async () => {
        this.loading = false;
      }));
    }

    if (component) {
      this.componentData = component;
      return;
    }

    this.waitingFor = Math.random();

    const componentData = getComponent(
      this.waitingFor,
      ({ component }) => {
        // Named param for making callback future proof
        runInAction(() => {
          if (component) {
            this.componentData = component;
          }
        });
      }
    );

    // In case returned value was a promise
    if (componentData && componentData.then) {

      // IIFE to check if a later ending promise was creating a race condition
      // Check test case for more info
      (check => {
        componentData.then(action((component: any) => {
          // Checks that the promise last invoked is the one we load
          if (this.waitingFor !== check) {
            return;
          }
          // component is `import` so call the default export with context
          this.componentData = component.default(this._context);
        })).catch(action((e: any) => {
          this.error = e;
        }));
      })(this.waitingFor);
    }
  }
}

export default function AsyncView(context: Context) {

  const store = new AsyncStore(context);

  return observer(class extends React.Component<AsyncViewProps, AsyncViewState> {

    public componentWillMount() {
        store.loadComponent(this.props.store, this.props.component, this.props.getComponent);
    }

    public render() {
      if (store.componentData && !store.loading) {
        return React.createElement(store.componentData, this.props);
      } else if (this.props.loading) {
        const loadingComponent = this.props.loading();
        return loadingComponent;
      }
      return (
        <div style={{
          height: '100%',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center'
        }}>
          <Fade
            in={true}
            unmountOnExit
          >
            <CircularProgress />
          </Fade>
        </div>
      );
    }
  });
}
