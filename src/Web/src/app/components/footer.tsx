import * as React from 'react';
import glamorous from 'glamorous';

interface FooterProps {
  version: string;
  title: string;
}

const FooterView = glamorous('footer')((_) => ({
  padding: 20,
  textAlign: 'center',
  // background: palette.primaryLight
}));
export default class extends React.Component<FooterProps, {}> {

  public render() {
    const year = new Date().getFullYear();
    const { title, version } = this.props;
    return (
      <FooterView>
        <div>
          {title} © {year} {version}
        </div>
      </FooterView>
    );
  }
}
