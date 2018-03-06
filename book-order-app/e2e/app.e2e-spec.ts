import { BookOrderAppPage } from './app.po';

describe('book-order-app App', () => {
  let page: BookOrderAppPage;

  beforeEach(() => {
    page = new BookOrderAppPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!!');
  });
});
