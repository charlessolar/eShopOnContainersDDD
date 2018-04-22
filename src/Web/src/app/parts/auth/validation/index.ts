export default {
  username: {
    presence: true,
    length: {
      minimum: 3,
      message: 'must be at least 3 characters'
    }
  },
  email: {
    presence: true,
    email: true,
    length: {
      minimum: 3,
      message: 'must be at least 3 characters'
    }
  },
  password: {
    presence: true,
    length: {
      minimum: 6,
      message: 'must be at least 6 characters'
    }
  },
  first_name: {
    presence: true,
    length: {
      maximum: 24,
      message: 'max length is 24 characters'
    }
  },
  last_name: {
    presence: true,
    length: {
      maximum: 24,
      message: 'max length is 24 characters'
    }
  }
};
