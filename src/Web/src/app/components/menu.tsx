import Button from '@material-ui/core/Button';
import ListItemIcon from '@material-ui/core/ListItemIcon';
import ListItemText from '@material-ui/core/ListItemText';
import Menu from '@material-ui/core/Menu';
import MenuItem from '@material-ui/core/MenuItem';
import ArrowDropDown from '@material-ui/icons/ArrowDropDown';
import ExitToApp from '@material-ui/icons/ExitToApp';
import Face from '@material-ui/icons/Face';
import * as React from 'react';

export interface Route {
  route: string;
  text?: string;
  icon?: any;
}

interface MenuProps {
  authenticated: boolean;
  name: string;
  navChange: (menu: Route) => void;
}

interface MenuState {
  anchorEl: HTMLElement;
}

export default class extends React.Component<MenuProps, MenuState> {

  private menus(authenticated): Route[] {
    if (authenticated) {
      return [
        {
          icon: (<Face />),
          route: '/profile',
          text: 'Profile'
        },
        {
          icon: (<ExitToApp />),
          route: '/logout',
          text: 'Logout'
        }
      ];
    }
    return [
      {
        route: '/login',
        text: 'Login'
      },
      {
        route: '/register',
        text: 'Register'
      }
    ];
  }

  constructor(props) {
    super(props);

    this.state = {
      anchorEl: null
    };
  }

  public handleMenu(event: React.MouseEvent<HTMLElement>) {
    this.setState({ anchorEl: event.currentTarget });
  }

  public handleClose() {
    this.setState({ anchorEl: null });
  }

  public render() {
    const { authenticated, name, navChange } = this.props;
    const { anchorEl } = this.state;
    const open = Boolean(anchorEl);

    return (
      <div>
        <Button onClick={(e) => this.handleMenu(e)} color='inherit'>
          {name} <ArrowDropDown />
        </Button>
        <Menu id='profile-appbar'
          anchorEl={anchorEl}
          anchorOrigin={{
            vertical: 'top',
            horizontal: 'right'
          }}
          transformOrigin={{
            vertical: 'top',
            horizontal: 'right'
          }}
          open={open}
          onClose={() => this.handleClose()}
        >
          {this.menus(authenticated)
            .map((menu, key) => (
              <MenuItem key={key} onClick={() => { navChange(menu); }}>
                <ListItemIcon>{menu.icon}</ListItemIcon>
                <ListItemText inset primary={menu.text} />
              </MenuItem>
            ))}
        </Menu>
      </div>
    );
  }

}
