import * as React from 'react';
import { observable, action } from 'mobx';
import { observer, inject } from 'mobx-react';
import Debug from 'debug';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';
import Snackbar from 'material-ui/Snackbar';
import IconButton from 'material-ui/IconButton';
import CloseIcon from 'material-ui-icons/Close';

import Alert from './alert';
import { StoreType } from '../stores';

const debug = new Debug('alerts');

interface AlertStackProps {
  store?: StoreType;
}

const style = (theme: Theme) => ({
  close: {
    width: theme.spacing.unit * 4,
    height: theme.spacing.unit * 4,
  }
});

@inject('store')
@observer
class AlertComponent extends React.Component<AlertStackProps & WithStyles<'close'>, {}> {

  @action
  private handleClose(id: string) {
    const { store } = this.props;
    store.alertStack.remove(id);
  }

  public render() {
    const { store, classes } = this.props;

    return (
      <div>
        {Array.from(store.alertStack.stack.values(), (message, key) => (
          <Snackbar
            key={key}
            anchorOrigin={{
              vertical: 'top',
              horizontal: 'right',
            }}
            open={true}
            autoHideDuration={6000}
          >
            <Alert key={key} type={message.type} message={message.message} action={
              <IconButton
                key='close'
                aria-label='Close'
                color='inherit'
                className={classes.close}
                onClick={(e) => this.handleClose(message.id)}
              >
                <CloseIcon />
              </IconButton>
            } />
          </Snackbar>
        ))}
      </div>
    );
  }
}

export default withStyles(style)(AlertComponent);
