import * as React from 'react';
import { hot } from 'react-hot-loader';

import { Theme, withStyles, WithStyles } from '@material-ui/core/styles';

import { DashboardStoreType, DashboardStoreModel } from '../stores/dashboard';

interface DashboardProps {
  store?: DashboardStoreType;
}

const styles = (theme: Theme) => ({

});

class DashboardView extends React.Component<DashboardProps & WithStyles<never>, {}> {

  public render() {
    return (
    <div>
      Hello world
      </div>
      );
  }
}

export default hot(module)(withStyles(styles)<DashboardProps>(DashboardView));
