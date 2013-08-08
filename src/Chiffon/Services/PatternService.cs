namespace Chiffon.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Narvalo;
    using Narvalo.Data;
    using Narvalo.Fx;

    public class PatternService : IPatternService
    {
        readonly SqlHelper _sqlHelper;

        public PatternService(SqlHelper sqlHelper)
            : base()
        {
            Requires.NotNull(sqlHelper, "sqlHelper");

            _sqlHelper = sqlHelper;
        }

        public IEnumerable<Pattern> GetShowcasedPatterns()
        {
            var patterns = new List<Pattern>();

            using (var cnx = _sqlHelper.CreateConnection()) {
                using (var cmd = new SqlCommand()) {
                    cmd.CommandText = "usp_GetShowcasedPatterns";
                    cmd.Connection = cnx;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cnx.Open();

                    using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
                        while (rdr.Read()) {
                            var designerKey = DesignerKey.Parse(rdr.GetStringColumn("designer"))
                                .ValueOrThrow(() => new Exception("XXX"));
                            var reference = rdr.GetStringColumn("reference");

                            var pattern = new Pattern(new PatternId(designerKey, reference)) {
                                CreationTime = rdr.GetDateTimeColumn("creation_time"),
                                Preferred = rdr.GetBooleanColumn("preferred"),
                                Published = true,
                                Showcased = true,
                            };

                            patterns.Add(pattern);
                        }
                    }
                }
            }

            return patterns;
        }

        //public Maybe<Tuple<PatternVisibility, PatternImage>> MayGetImage(
        //    DesignerKey designerKey, string reference, PatternSize size)
        //{
        //    var result = Maybe<Pattern>.None;

        //    using (var cnx = _sqlHelper.CreateConnection()) {
        //        using (var cmd = new SqlCommand()) {
        //            cmd.CommandText = "usp_GetPattern";
        //            cmd.Connection = cnx;
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            SqlParameterCollection p = cmd.Parameters;
        //            p.Add("@reference", SqlDbType.NVarChar).Value = reference;
        //            p.Add("@designer", SqlDbType.NVarChar).Value = designerKey.Key;

        //            cnx.Open();

        //            using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection)) {
        //                if (rdr.Read()) {
        //                    var pattern = new Pattern(new PatternId(designerKey, reference)) {
        //                        //CreationTime = rdr.GetDateTimeColumn("creation_time"),
        //                        Preferred = rdr.GetBooleanColumn("preferred"),
        //                        Published = rdr.GetBooleanColumn("online"),
        //                        Showcased = rdr.GetBooleanColumn("showcased"),
        //                    };
        //                    result = Maybe.Create(pattern);
        //                }
        //            }
        //        }
        //    }

        //    return result.Map(_ => Tuple.Create(_.GetVisibility(size), _.GetImage(size)));
        //}
    }
}
