import Debug from 'debug';

export default function(enable?: boolean) {
    if (enable) {
        Debug.enable('*,-engine*,-sockjs-client*,-socket*');
    } else {
        Debug.disable();
    }
}
