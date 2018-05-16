import * as React from 'react';
import { observer } from 'mobx-react';
import { hot } from 'react-hot-loader';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';

import { Using, Formatted } from '../../../components/models';
import { BasketStoreType, BasketStoreModel } from '../stores/basket';

interface BasketProps {
  store?: BasketStoreType;
}

const styles = (theme: Theme) => ({

});

@observer
class BasketView extends React.Component<BasketProps & WithStyles<never>, {}> {

  public render() {
    const { store, classes } = this.props;

    return (
      <></>
    );
  }
}

export default hot(module)(withStyles(styles as any)(BasketView));
