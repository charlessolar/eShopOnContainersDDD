import red from '@material-ui/core/colors/red';
import { createMuiTheme } from '@material-ui/core/styles';

const theme = createMuiTheme({
  palette: {
    primary: {
      light: '#6f74dd ',
      main: '#3949ab',
      dark: '#00227b',
      contrastText: '#fafafa'
    },
    secondary: {
      light: '#62727b ',
      main: '#37474f',
      dark: '#102027',
      contrastText: '#fafafa'
    },
    error: red
  },
});

export default () => theme;
