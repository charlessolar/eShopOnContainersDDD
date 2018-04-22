import * as React from 'react';
import { observer } from 'mobx-react';

import { Theme, withStyles, WithStyles } from 'material-ui/styles';
import Paper from 'material-ui/Paper';
import { FormControl, FormHelperText } from 'material-ui/Form';
import TextField from 'material-ui/TextField';
import Button from 'material-ui/Button';
import Grid from 'material-ui/Grid';
import Typography from 'material-ui/Typography';

import input from '../../../components/inputs/input';
import alertAjax from '../../../components/alertAjax';
import { Context } from '../../../context';
import { LoginStore } from '../stores/login';

interface LoginProps {
  store: LoginStore;
}

export default function Login(context: Context) {

  const styles = (theme: Theme) => ({
    container: {
      textAlign: 'center',
      width: 600,
      margin: 20,
      padding: 30,
      display: 'flex',
    },
    textField: {
      marginLeft: theme.spacing.unit,
      marginRight: theme.spacing.unit,
      width: 400,
    },
    menu: {
      width: 200,
    },
  });

  const Input = input(context);
  const AlertAjax = alertAjax(context);

  return withStyles(styles)(observer(class extends React.Component<LoginProps & WithStyles<'container' | 'textField' | 'menu'>, {}> {

    public render() {
      const { store, classes } = this.props;

      return (
        <Paper className={classes.container} elevation={3}>
          <AlertAjax response={store.error} />
          <form autoComplete='off'>
            <img src={require('../img/logo.png')} height={128} width={128} /><br />
            <Grid container spacing={24}>
              <Grid item xs={12}>
                  <Input id='email' required error={store.validation.email} label='Email Address' autoComplete='email' onChange={val => {
                    store.email = val;
                  }} />
                  <Input id='password' required error={store.validation.password} label='Password' type='password' autoComplete='current-password' onChange={val => {
                    store.password = val;
                  }} />
              </Grid>
              <Grid item xs={12}>
                <Button variant='raised' color='primary' fullWidth onClick={() => {
                  store.login();
                }}>Sign In</Button>
              </Grid>
              <Grid item xs={12}>
                <Typography variant='headline' component='h3'>
                  Don't have a eShopOnContainers account? Sign Up
                </Typography>
                <Typography component='aside'>
                  Forgot Password
                </Typography>
              </Grid>
            </Grid>
          </form>
        </Paper>
      );
    }
  }));
}
