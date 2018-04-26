import * as React from 'react';
import { observable, action, runInAction } from 'mobx';
import { observer, inject } from 'mobx-react';

import Fade from 'material-ui/transitions/Fade';
import { CircularProgress } from 'material-ui/Progress';

class AsyncStore {

  @observable
  public loading: boolean;
  @observable
  public error: boolean;
  @observable
  public componentData: any;

  private loadedStore: string;

  private waitingFor: number;

  @action
  public loadComponent(promiseAction?: () => Promise<{}>, component?: any, getComponent?: (check?: number, cb?: (props: AsyncViewProps) => void) => Promise<any>) {
    // cheap hack to not reload async route unless store name changes
    if (this.loadedStore && promiseAction.constructor.name === this.loadedStore) {
      return;
    }
    this.loadedStore = promiseAction.constructor.name;

    if (promiseAction) {
      this.loading = true;
      promiseAction().then(action(() => {
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
        componentData.then((component: any) => {
          // Checks that the promise last invoked is the one we load
          if (this.waitingFor !== check) {
            return;
          }
          runInAction(() => {
            // component is `import`
            this.componentData = component.default;
          });
        }).catch(action((e: any) => {
          this.error = e;
        }));
      })(this.waitingFor);
    }
  }
}

interface AsyncViewProps {
  async?: AsyncStore;
  url?: string;
  component?: any;
  action?: () => Promise<{}>;

  getComponent?: (check?: number, cb?: (props: AsyncViewProps) => void) => Promise<any>;
  loading?: () => any;
}
interface AsyncViewState {
  componentData: any;
  error: any;
  loading: boolean;
}

@inject((stores, props) => { props['async'] = new AsyncStore(); return props; })
@observer
export default class AsyncView extends React.Component<AsyncViewProps, AsyncViewState> {

  public componentWillMount() {
    const { async } = this.props;
    async.loadComponent(this.props.action, this.props.component, this.props.getComponent);
  }

  public render() {
    const { async } = this.props;
    if (async.componentData && !async.loading) {
      return React.createElement(async.componentData, this.props);
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
}
