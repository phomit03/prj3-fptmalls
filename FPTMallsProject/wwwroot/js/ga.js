function setCookie(cname, cvalue, attributes) {
  const d = new Date();

  const { path, domain, secure, expires } = attributes;

  d.setTime(d.getTime() + (expires*60*1000));
  let exp = "expires="+ d.toUTCString();
  document.cookie = cname + "=" + cvalue + ";" + exp + `;path=${path};domain=${domain};secure=${secure}`;
}

function getCookie(cname) {
  let name = cname + "=";
  let decodedCookie = decodeURIComponent(document.cookie);
  let ca = decodedCookie.split(';');
  for(let i = 0; i <ca.length; i++) {
    let c = ca[i];
    while (c.charAt(0) == ' ') {
      c = c.substring(1);
    }
    if (c.indexOf(name) == 0) {
      return c.substring(name.length, c.length);
    }
  }
  return "";
}

/********************************************
 * Analytics Tracking
 *
 * Enhancements for Google Analytics 4
 ********************************************/
(function uniquePageview() {
  let uniquePageviews = getCookie("unique-pageviews");

  if (uniquePageviews) {
    uniquePageviews = JSON.parse(uniquePageviews);
  } else {
    uniquePageviews = [];
  }

  if (uniquePageviews.indexOf(location.href) !== -1) {
    return;
  }

  // Track a pageview for a newly visited URL in this session
  uniquePageviews.push(location.href);
  uniquePageviews = JSON.stringify(uniquePageviews);

  setCookie("unique-pageviews", uniquePageviews, {
    path: "/",
    domain: location.hostname,
    secure: true,
    expires: 30,
  });

    // Send "unique_page_view" event to GA4 using gtag
    window.gtag('event', 'unique_page_view', {
    page_location: document.location.href,
    page_referrer: document.referrer,
  });
  
  // Send "unique_page_view" event to GA4 using dataLayer
  const properties = {
    event: "unique_page_view",
    page_location: document.location.href,
    page_referrer: document.referrer,
  };
  window.dataLayer.push(properties);
  

})();
